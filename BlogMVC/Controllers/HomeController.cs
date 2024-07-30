
using BlogMVC.Helpers;
using BlogMVC.Models;
using BlogMVC.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;
using System.Diagnostics;
using System.Drawing.Printing;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbBlogContext _db;
        public HomeController(DbBlogContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<News> getLstCate = _db.News.OrderByDescending(x => x.PostId).ToList();
            return View(getLstCate);
        }

    }
}