

namespace BlogMVC.ModelViews
{
    public class ShareViewModels
    {
    }
    public class Paging
    {
        public Paging(int totalItems, int? page, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            var currentPages = page != null ? page : 1;
            var startPages = currentPages - 5;
            var endPages = currentPages + 4;
            if (startPages <= 0)
            {
                endPages -= (startPages - 1);
                startPages = 1;
            }
            if (endPages > totalPages)
            {
                endPages = totalPages;
                if (endPages > 10)
                {
                    startPages = endPages - 9;
                }
            }
            TotalIteams = totalItems;
            Page = page;
            PageSize = pageSize;
            CurrentPage = currentPages;
            TotalPage = totalPages;
            StartPage = startPages;
            EndPage = endPages;

        }

        public int TotalIteams { get; private set; }
        public int? Page { get; private set; }
        public int PageSize { get; private set; }
        public int? CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int? StartPage { get; private set; }
        public int? EndPage { get; private set; }
    }
}
