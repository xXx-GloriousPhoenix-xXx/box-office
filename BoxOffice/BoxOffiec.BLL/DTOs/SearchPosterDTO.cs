using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class SearchPostersDto
    {
        public string? AuthorName { get; set; }
        public string? Title { get; set; }
        public PosterGenre? Genre { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
    }
}