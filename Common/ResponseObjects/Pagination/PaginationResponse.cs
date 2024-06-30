namespace Common.ResponseObjects.Pagination
{
    public class PaginationResponse
    {
        public IEnumerable<object>? Items { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
