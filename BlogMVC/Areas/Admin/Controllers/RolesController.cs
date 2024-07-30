using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using BlogMVC.Helpers;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using BlogMVC.Areas.Admin.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomerRoles.AdministratorOrUser)]
    public class RolesController : Controller
    {
        private readonly DbBlogContext _db;
        public INotyfService _notyfService;

        public RolesController(DbBlogContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Uitilities.PAGE_SIZE;
            var pageSize = 1;
            var lstRole = _db.Roles.OrderByDescending(x => x.RoleId);

            PagedList<Role> models = new PagedList<Role>(lstRole, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            _notyfService.Error("OK");
            return View(models);
        }

        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Roles == null)
            {
                return NotFound();
            }

            var role = await _db.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,RoleDescription")] Role role)
        {
            if (ModelState.IsValid)
            {
                _db.Add(role);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Roles == null)
            {
                return NotFound();
            }

            var role = await _db.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,RoleDescription")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(role);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

        // GET: Admin/Roles/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }*/

        // POST: Admin/Roles/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Role role = await _db.Roles.FindAsync(id);
            if (role == null)
            {
                return Json(new { success = false, status = "Invalid ID!" });
            }
            try
            {
                if (role != null)
                {
                    _db.Roles.Remove(role);
                }

                await _db.SaveChangesAsync();
                return Json(new { success = true, status = "Delete Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, status = ex.Message.ToString() });
            }
        }

        private bool RoleExists(int id)
        {
            return (_db.Roles?.Any(e => e.RoleId == id)).GetValueOrDefault();
        }
    }
}
