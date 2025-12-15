using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class GetPosterWithTicketsDto : GetPosterDto
    {
        public required PagedResponse<GetTicketAdditionDto> Tickets { get; set; }
    }
}
