using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class PosterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AuthorDto Author { get; set; } = null!;
        public List<PosterGenre> Genres { get; set; } = new();
        public DateOnly ReleaseDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public IEnumerable<TicketTypeInfoDto> TicketTypes { get; set; } = [];
    }
}