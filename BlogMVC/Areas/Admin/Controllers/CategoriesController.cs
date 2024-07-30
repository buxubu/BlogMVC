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
using System.Data;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomerRoles.AdministratorOrUser)]
    public class CategoriesController : Controller
    {
        private readonly DbBlogContext _db;
        public readonly IMapper _mapper;

        public CategoriesController(DbBlogContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Uitilities.PAGE_SIZE;
            var pageSize = 1;
            var lstCat = _db.Categories.OrderByDescending(x => x.CaId);

            PagedList<Category> models = new PagedList<Category>(lstCat, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Categories == null)
            {
                return NotFound();
            }

            var category = await _db.Categories
                .FirstOrDefaultAsync(m => m.CaId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            ViewData["DanhMucGoc"] = new SelectList(_db.Categories.Where(x => x.Levels == 1), "CaId", "CatName");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CategoriesViewModel category)
        {
            if (ModelState.IsValid)
            {
                category.Alias = Uitilities.SEOUrl(category.CatName);
                if (category.Parents == null)
                {
                    category.Levels = 1;
                }
                else
                {
                    category.Levels = category.Parents == 0 ? 1 : 2;
                }
                if (category.fThumb != null)
                {
                    string extension = Path.GetExtension(category.fThumb.FileName);
                    var up = await Uitilities.UploadFileToFolderImages(category.fThumb);
                    category.Thumb = up;
                }
                if (category.fIcon != null)
                {
                    string extension = Path.GetExtension(category.fIcon.FileName);
                    category.Icon = await Uitilities.UploadFileToFolderImages(category.fIcon);
                }
                if (category.fCover != null)
                {
                    string extension = Path.GetExtension(category.fCover.FileName);
                    category.Cover = await Uitilities.UploadFileToFolderImages(category.fCover);
                }
                _db.Categories.Add(_mapper.Map<Category>(category));
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Categories == null)
            {
                return NotFound();
            }

            var category = await _db.Categories.FindAsync(id);
            var map = _mapper.Map<CategoriesViewModel>(category);
            if (category == null)
            {
                return NotFound();
            }
            return View(map);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriesViewModel category)
        {
            if (id != category.CaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var findCate = await _db.Categories.FindAsync(id);
                    if (findCate == null) { return NotFound(); }
                    if (category.fThumb.FileName != findCate.Thumb)
                    {
                        category.Thumb = await Uitilities.UpdateFileToFolderImages(category.fThumb, findCate.Thumb);
                    }

                    findCate.CaId = category.CaId;
                    findCate.CatName = category.CatName;
                    findCate.Title = category.Title;
                    findCate.MetaDesc = category.MetaDesc;
                    findCate.MetaKey = category.MetaKey;
                    findCate.Thumb = category.Thumb;
                    findCate.Published = category.Published;
                    findCate.Ordering = category.Ordering;
                    findCate.Parents = category.Parents;
                    findCate.Levels = category.Levels;
                    findCate.Icon = category.Icon;
                    findCate.Cover = category.Cover;
                    findCate.Description = category.Description;
                    findCate.Alias = category.Alias;

                    _db.Categories.Update(findCate);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CaId))
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        /* public async Task<IActionResult> Delete(int? id)
         {
             if (id == null || _db.Categories == null)
             {
                 return NotFound();
             }

             var category = await _db.Categories
                 .FirstOrDefaultAsync(m => m.CaId == id);
             if (category == null)
             {
                 return NotFound();
             }

             return View(category);
         }*/

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_db.Categories == null)
            {
                return Problem("Entity set 'DbBlogContext.Categories'  is null.");
            }
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                Uitilities.DeleteFileImages(category.Thumb);
                _db.Categories.Remove(category);
            }

            await _db.SaveChangesAsync();
            return Json(Ok());
        }

        private bool CategoryExists(int id)
        {
            return (_db.Categories?.Any(e => e.CaId == id)).GetValueOrDefault();
        }
    }
}
