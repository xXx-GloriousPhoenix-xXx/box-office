using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class GetPosterWithTicketInfosDto : GetPosterDto
    {
        public required ICollection<GetTicketInfoAdditionDto> TicketInfos { get; set; }
    }
}
