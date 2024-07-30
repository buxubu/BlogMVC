using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using BlogMVC.Areas.Admin.Models;
using BlogMVC.Services.ICatePro;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CateProductsController : Controller
    {
        private readonly DbBlogContext _db;
        private readonly ICatePro _cateProServices;

        public CateProductsController(DbBlogContext db, ICatePro cateProServices)
        {
            _db = db;
            _cateProServices = cateProServices;
        }

        // GET: Admin/CateProducts
        public async Task<IActionResult> Index()
        {
            return _db.CateProducts != null ?
                        View(await _db.CateProducts.ToListAsync()) :
                        Problem("Entity set 'DbBlogContext.CateProducts'  is null.");
        }

        // GET: Admin/CateProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.CateProducts == null)
            {
                return NotFound();
            }

            var cateProduct = await _db.CateProducts
                .FirstOrDefaultAsync(m => m.IdCatePro == id);
            if (cateProduct == null)
            {
                return NotFound();
            }

            return View(cateProduct);
        }

        // GET: Admin/CateProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMucGoc"] = new SelectList(_db.CateProducts.Where(x => x.Level == 1), "IdCatePro", "CatName");
            return View();
        }

        // POST: Admin/CateProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _cateProServices.CreateCateProAsync(model);
            }
            return RedirectToAction("Index", "CateProducts", new { Area = "Admin" });
        }

        // GET: Admin/CateProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.CateProducts == null)
            {
                return NotFound();
            }

            var cateProduct = await _db.CateProducts.FindAsync(id);
            if (cateProduct == null)
            {
                return NotFound();
            }
            return View(cateProduct);
        }

        // POST: Admin/CateProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCatePro,CatName,ParentId,Level,Published,Thumb,Title,Alias,MetaDesc,MetaKey,Cover,SchemaMakup")] CateProduct cateProduct)
        {
            if (id != cateProduct.IdCatePro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(cateProduct);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CateProductExists(cateProduct.IdCatePro))
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
            return View(cateProduct);
        }

        // GET: Admin/CateProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.CateProducts == null)
            {
                return NotFound();
            }

            var cateProduct = await _db.CateProducts
                .FirstOrDefaultAsync(m => m.IdCatePro == id);
            if (cateProduct == null)
            {
                return NotFound();
            }

            return View(cateProduct);
        }

        // POST: Admin/CateProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.CateProducts == null)
            {
                return Problem("Entity set 'DbBlogContext.CateProducts'  is null.");
            }
            var cateProduct = await _db.CateProducts.FindAsync(id);
            if (cateProduct != null)
            {
                _db.CateProducts.Remove(cateProduct);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CateProductExists(int id)
        {
            return (_db.CateProducts?.Any(e => e.IdCatePro == id)).GetValueOrDefault();
        }
    }
}
