using BlogMVC.Enums;
using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogMVC.Controllers.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly DbBlogContext _db;
        private IMemoryCache _memoryCache;
        public CategoryViewComponent(DbBlogContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }
        public IViewComponentResult Invoke()
        {
            //var lstCate = _memoryCache.GetOrCreate(CacheKeys.Categories, entry =>
            //{
            //    entry.SlidingExpiration = TimeSpan.MaxValue;
            //    return GetLstCate();
            //});
            return View(GetLstCate());
        }

        public IEnumerable<Category> GetLstCate()
        {
            IEnumerable<Category> lstCate = _db.Categories.ToList();
            return lstCate;
        }
    }
}
