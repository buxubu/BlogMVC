namespace BlogMVC.Areas.Admin.Models
{
    public class CategoriesViewModel
    {
        public int CaId { get; set; }

        public string? CatName { get; set; }

        public string? Title { get; set; }

        public string? MetaDesc { get; set; }

        public string? MetaKey { get; set; }

        public string? Thumb { get; set; }

        public IFormFile fThumb { get; set; }

        public bool Published { get; set; }

        public int? Ordering { get; set; }

        public int? Parents { get; set; }

        public int? Levels { get; set; }

        public string? Icon { get; set; }

        public IFormFile? fIcon { get; set; }

        public string? Cover { get; set; }

        public IFormFile? fCover { get; set; }

        public string? Description { get; set; }
        
        public string? Alias { get; set; }

    }
}
