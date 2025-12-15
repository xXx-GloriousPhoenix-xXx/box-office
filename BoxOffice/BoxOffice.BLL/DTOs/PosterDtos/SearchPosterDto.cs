using BoxOffice.BLL.DTOs.DtoEnums;

namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class SearchPosterDto
    {
        public string? Name { get; set; }
        public string? Venue { get; set; }
        public string? AuthorName { get; set; }
        public ICollection<string>? Genres { get; set; }

        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        public int? DurationMin { get; set; }
        public int? DurationMax { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        public PosterSortCriteria? SearchCriteria { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
