using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class UpdatePosterDto
    {
        public string? Name { get; set; }
        public List<PosterGenre>? Genres { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? Venue { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Description { get; set; }
    }
}