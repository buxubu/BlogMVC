using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using BlogMVC.ModelViews;
using BlogMVC.Services.IProducts;

namespace BlogMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DbBlogContext _db;
        private readonly IProducts _productServices;

        public ProductsController(DbBlogContext db, IProducts productServices)
        {
            _db = db;
            _productServices = productServices;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(_productServices.GetAllProduct());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Products == null)
            {
                return NotFound();
            }

            var product = await _db.Products
                .Include(p => p.IdCateProNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["IdCatePro"] = new SelectList(_db.CateProducts, "IdCatePro", "CatName");
            ViewData["IdColor"] = new SelectList(_db.Colors, "IdColor", "Color1");
            ViewData["IdSize"] = new SelectList(_db.Sizes, "IdSize", "Size1");
            ViewData["IdStock"] = new SelectList(_db.Stocks, "IdStock", "Stock1");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            await _productServices.CreateProductAsync(product);
            return RedirectToAction("Index", "Products", new { Area = "Admin" });
        }
        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Products == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdCatePro"] = new SelectList(_db.CateProducts, "IdCatePro", "IdCatePro", product.IdCatePro);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduct,IdCatePro,NameProduct,Price,Discount,Descrip,Video,CreateDate,BestSaller,HomeFlag,Active,Tag,Alias,MetaDesc,MetaKey")] Product product)
        {
            if (id != product.IdProduct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.IdProduct))
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
            ViewData["IdCatePro"] = new SelectList(_db.CateProducts, "IdCatePro", "IdCatePro", product.IdCatePro);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Products == null)
            {
                return NotFound();
            }

            var product = await _db.Products
                .Include(p => p.IdCateProNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Products == null)
            {
                return Problem("Entity set 'DbBlogContext.Products'  is null.");
            }
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_db.Products?.Any(e => e.IdProduct == id)).GetValueOrDefault();
        }
    }
}
