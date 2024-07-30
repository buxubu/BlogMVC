namespace BlogMVC.Areas.Admin.Models
{
    public class NewsViewModel
    {
        public int PostId { get; set; }

        public int CaId { get; set; }

        public int AccountId { get; set; }

        public string? Title { get; set; }

        public string? ShortContent { get; set; }

        public string? Contents { get; set; }

        public string? Thumb { get; set; }

        public bool Published { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Author { get; set; }

        public string? Tags { get; set; }

        public bool IsHot { get; set; }

        public bool IsNewFeed { get; set; }

        public IFormFile? fThumb { get; set; }
    }
}
