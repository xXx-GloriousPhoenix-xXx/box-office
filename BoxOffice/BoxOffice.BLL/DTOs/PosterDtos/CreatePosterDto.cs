using BoxOffice.BLL.DTOs.TicketInfoDtos;

namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class CreatePosterDto
    {
        public required string Name { get; set; }
        public required Guid AuthorId { get; set; }
        public required string Description { get; set; }
        public required string Venue { get; set; }
        public required DateOnly Date { get; set; }
        public required int Duration { get; set; }
        public required ICollection<string> Genres { get; set; }
        public required ICollection<CreateTicketInfoAdditionDto> TicketInfos { get; set; }
    }
}
