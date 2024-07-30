namespace BlogMVC.ModelViews
{
    public class ProductViewModel
    {
        public int IdProduct { get; set; }

        public int IdCatePro { get; set; }
        public string? NameCate { get; set; }
        public int? IdThumbPro { get; set; }
        public string? NameThumbPro { get; set; }

        public int IdSize { get; set; }
        public string? Size { get; set; }

        public int IdColor { get; set; }
        public string? Color { get; set; }

        public int IdStock { get; set; }
        public int? Stock { get; set; }

        public string? NameProduct { get; set; }

        public int? Price { get; set; }

        public int? Discount { get; set; }

        public string? Descrip { get; set; }

        public string? Video { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool BestSaller { get; set; }

        public bool HomeFlag { get; set; }

        public bool Active { get; set; }

        public string? Tag { get; set; }

        public string? Alias { get; set; }

        public string? MetaDesc { get; set; }

        public string? MetaKey { get; set; }
        public List<IFormFile>? fThumb { get; set; }
        public IFormFile fVideo { get; set; }
    }
}
