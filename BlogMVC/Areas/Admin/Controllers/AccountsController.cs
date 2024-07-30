using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using BlogMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.Areas.Admin.Models;
using BlogMVC.Extentions;
using AutoMapper;
using PagedList.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly DbBlogContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public AccountsController(DbBlogContext db, IMapper mapper, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _accessor = accessor;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index(int? page)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }


            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Uitilities.PAGE_SIZE;
            var pageSize = 1;
            var dbBlogContext = _db.Accounts.Include(a => a.Role);

            PagedList<Account> models = new PagedList<Account>(dbBlogContext, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [HttpGet]
        [AllowAnonymous]
        /*[Route("/dang-nhap.html", Name = "Login")]*/
        public async Task<IActionResult> Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "News", new { Area = "Admin" });
            }


            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID != null) return RedirectToAction("Index", "News", new { Area = "Admin" });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        /*[Route("/dang-nhap.html", Name = "Login")]*/
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkAccount = await _db.Accounts.Include(x => x.Role).SingleOrDefaultAsync(x => x.Email == model.Email.ToLower().Trim());

                    if (checkAccount == null)
                    {
                        ViewBag.Error = "Login information is incorrect";
                        return View(model);
                    }

                    bool pass = (Share.ToMD5(model.Password.Trim()) + checkAccount.Salt) == checkAccount.Password;
                    if (!pass)
                    {
                        ViewBag.Error = "Login information is incorrect";
                        return View(model);
                    }

                    checkAccount.LastLogin = DateTime.Now;

                    _db.Accounts.Update(checkAccount);
                    await _db.SaveChangesAsync();

                    //save session
                    HttpContext.Session.SetString("AccountId", checkAccount.AccountId.ToString());


                    //identity
                    var accountClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, checkAccount.FullName,ClaimValueTypes.String),
                    new Claim(ClaimTypes.Email, checkAccount.Email,ClaimValueTypes.String),
                    new Claim("AccountId", checkAccount.AccountId.ToString(),ClaimValueTypes.Integer),
                    new Claim("RoleId", checkAccount.RoleId.ToString(),ClaimValueTypes.Integer),
                    new Claim(ClaimTypes.Role, checkAccount.Role.RoleName),
                };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(accountClaims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                        );

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = model.KeepMeLogin
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);


                    ViewData["ErrorClaims"] = "Error to save claims";
                    return RedirectToAction("Index", "News", new { Area = "Admin" });
                }

                return RedirectToAction("Index", "News", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }


            var accountID = HttpContext.Session.GetString("AccountId");
            if (accountID == null) return NotFound();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            Account checkAccount = await _db.Accounts.Where(x => x.AccountId == int.Parse(accountId)).FirstOrDefaultAsync();
            if (checkAccount == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            if (ModelState.IsValid)
            {
                if (!model.EmailNow.Equals(checkAccount.Email))
                {
                    ViewBag.Error = "Login information is incorrect";
                    return View(model);
                }

                bool pass = (Share.ToMD5(model.PasswordNow.Trim()) + checkAccount.Salt) == checkAccount.Password;
                if (!pass)
                {
                    ViewBag.Error = "Login information is incorrect";
                    return View(model);
                }
                if (model.PasswordNew != model.ConfirmPassword) ViewBag.Error = "Login information is incorrect";
                checkAccount.Salt = Share.GetSailt();
                checkAccount.Password = Share.ToMD5(model.PasswordNew) + checkAccount.Salt;
                _db.Accounts.Update(checkAccount);
                await _db.SaveChangesAsync();

                this.LogOut();
            }
            return View();
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_db.Roles, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountsViewModel account)
        {
            if (ModelState.IsValid)
            {
                account.Salt = Share.GetSailt();
                account.Password = Share.ToMD5(account.Password) + account.Salt;
                account.CreateDate = DateTime.Now;
                account.LastLogin = null;
                _db.Accounts.Add(_mapper.Map<Account>(account));
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(account);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_db.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FullName,Email,Phone,Password,Salt,Active,CreateDate,LastLogin,RoleId")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(account);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_db.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Accounts == null)
            {
                return Problem("Entity set 'DbBlogContext.Accounts'  is null.");
            }
            var account = await _db.Accounts.FindAsync(id);
            if (account != null)
            {
                _db.Accounts.Remove(account);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return (_db.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
