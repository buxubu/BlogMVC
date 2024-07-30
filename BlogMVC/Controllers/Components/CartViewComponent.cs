using BlogMVC.Enums;
using BlogMVC.Extentions;
using BlogMVC.Models;
using BlogMVC.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogMVC.Controllers.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly DbBlogContext _db;
        private IMemoryCache _memoryCache;
        public CartViewComponent(DbBlogContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
