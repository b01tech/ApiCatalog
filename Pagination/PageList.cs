namespace ApiCatalog.Pagination
{
    public class PageList<T> : List<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageList(List<T> list, int count, int pageSize, int pageNumber)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(list);
        }

        public static PageList<T> ToPageList(IQueryable<T> source, int pageSize, int pageNumber)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var pg = new PageList<T>(items, count, pageSize, pageNumber);
            return pg;
        }
    }
}
