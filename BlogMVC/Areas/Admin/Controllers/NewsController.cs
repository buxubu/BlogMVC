using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using PagedList.Core;
using BlogMVC.Areas.Admin.Models;
using BlogMVC.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly DbBlogContext _db;
        private readonly IMapper _mapper;

        public NewsController(DbBlogContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index(int? page)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            Account checkAccount = await _db.Accounts.Where(x => x.AccountId == int.Parse(accountId)).FirstOrDefaultAsync();
            if (checkAccount == null) return NotFound();

            //phan biet admin
            List<News> lstNews = new List<News>();
            if (checkAccount.RoleId == 15)
            {
                lstNews = await _db.News.Include(x => x.Account).Include(x => x.Ca).ToListAsync();
            }
            else
            {
                lstNews = await _db.News.Where(x => x.AccountId == checkAccount.AccountId).ToListAsync();
            }

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Uitilities.PAGE_SIZE;

            PagedList<News> models = new PagedList<News>(lstNews.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.News == null)
            {
                return NotFound();
            }

            var news = await _db.News
                .Include(n => n.Account)
                .Include(n => n.Ca)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            ViewData["AccountId"] = new SelectList(_db.Accounts, "AccountId", "FullName");
            ViewData["CaId"] = new SelectList(_db.Categories, "CaId", "CatName");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsViewModel news)
        {

            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            Account checkAccount = await _db.Accounts.Where(x => x.AccountId == int.Parse(accountId)).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                news.AccountId = checkAccount.AccountId;
                news.Contents = Uitilities.RemoveLinks(news.Contents);
                news.Contents = Uitilities.RemoveLinksStyle(news.Contents);
                news.Author = checkAccount.FullName;
                news.Alias = Uitilities.SEOUrl(news.Title);
                news.Tags = Uitilities.SEOUrl(news.ShortContent);
                news.CreateDate = DateTime.Now;
                ViewData["CaId"] = new SelectList(_db.Categories, "CaId", "CaId", news.CaId);
                if (news.CaId == 0) news.CaId = 1;

                if (news.fThumb != null)
                {
                    /*string extension = Path.GetExtension(news.fThumb.FileName);*/
                    news.Thumb = await Uitilities.UploadFileToFolderImages(news.fThumb);
                }
                _db.News.Add(_mapper.Map<News>(news));
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.News == null)
            {
                return NotFound();
            }

            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var news = await _db.News.Include(x => x.Ca).Where(x => x.PostId == id).FirstOrDefaultAsync();
            ViewData["AccountId"] = new SelectList(_db.Accounts, "AccountId", "AccountId", news.AccountId);
            ViewData["CaId"] = new SelectList(_db.Categories, "CaId", "CaId", news.CaId);

            var mapNews = _mapper.Map<NewsViewModel>(news);
            if (news == null)
            {
                return NotFound();
            }

            return View(mapNews);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsViewModel news)
        {

            ClaimsPrincipal claimUser = HttpContext.User;
            if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            //lay session
            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            //sau khi lấy session accountId tim kiem object
            Account checkAccount = await _db.Accounts.Where(x => x.AccountId == int.Parse(accountId)).FirstOrDefaultAsync();
            News getNews = await _db.News.FindAsync(id);

            //neu nguoi do la admin thi co quyen sua tat ca bai news
            if (!checkAccount.FullName.Equals("Nguyễn Văn Phúc"))
            {
                //check xem nguoi dang edit co dung news cua minh khong hay bai nguoi khac
                if (news.AccountId != checkAccount.AccountId) return RedirectToAction(nameof(Index));
            }


            if (ModelState.IsValid)
            {
                try
                {

                    ViewData["AccountId"] = new SelectList(_db.Accounts, "AccountId", "AccountId", news.AccountId);
                    ViewData["CaId"] = new SelectList(_db.Categories, "CaId", "CaId", news.CaId);

                    getNews.PostId = id;
                    getNews.AccountId = checkAccount.AccountId;
                    getNews.Contents = Uitilities.RemoveLinks(news.Contents);
                    getNews.Contents = Uitilities.RemoveLinksStyle(getNews.Contents);
                    getNews.Author = checkAccount.FullName;
                    getNews.Alias = news.Alias;
                    getNews.CreateDate = news.CreateDate;
                    getNews.CaId = news.CaId;
                    getNews.Title = news.Title;
                    getNews.ShortContent = news.ShortContent;
                    getNews.Published = news.Published;
                    getNews.Tags = news.Tags;
                    getNews.IsHot = news.IsHot;
                    getNews.IsNewFeed = news.IsNewFeed;

                    if (news.fThumb != null)
                    {
                        /*string extension = Path.GetExtension(news.fThumb.FileName);*/
                        news.Thumb = await Uitilities.UploadFileToFolderImages(news.fThumb);
                    }
                    else
                    {
                        getNews.Thumb = getNews.Thumb;

                    }
                    _db.News.Update(getNews);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.PostId))
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
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.News == null)
            {
                return NotFound();
            }

            var news = await _db.News
                .Include(n => n.Account)
                .Include(n => n.Ca)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.News == null)
            {
                return Problem("Entity set 'DbBlogContext.News'  is null.");
            }
            var news = await _db.News.FindAsync(id);
            if (news != null)
            {
                _db.News.Remove(news);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return (_db.News?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
