using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class CreatePosterDto
    {
        public required string Name { get; set; }
        public Guid AuthorId { get; set; }
        public List<PosterGenre> Genres { get; set; } = new();
        public DateOnly ReleaseDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<CreateTicketTypeInfoDto> TicketTypes { get; set; } = new();
    }
}