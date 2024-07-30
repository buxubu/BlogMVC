using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Controllers
{
    public class NewController : Controller
    {
        private readonly DbBlogContext _db;
        public NewController(DbBlogContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            return View(await _db.News.Where(x => x.CaId == id).ToListAsync());
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _db.News.Include(x => x.Ca).Where(x => x.PostId == id).FirstOrDefaultAsync());
        }
    }
}
