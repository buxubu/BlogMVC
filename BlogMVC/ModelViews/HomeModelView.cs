using BlogMVC.Models;

namespace BlogMVC.ModelViews
{
    public class HomeModelView
    {
        public IEnumerable<News> LstNew {  get; set; }
        public IEnumerable<Category> LstCategory { get; set; }

    }
}
