namespace BoxOffice.BLL.DTOs.AdditionalDtos
{
    public class PagedResponse<T>
    {
        public ICollection<T> Items { get; set; } = [];
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
