using BlogMVC.Models;

namespace BlogMVC.ModelViews
{
    public class DetailProduct
    {
        public ProductViewModel? Product { get; set; }
        public IEnumerable<ThumbProduct> LstThumb { get; set; }
        public IEnumerable<Size>? Size { get; set; }
        public IEnumerable<Color>? Color { get; set; }
        public Stock? Stock { get; set; }
    }
}
