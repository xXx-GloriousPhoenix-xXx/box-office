using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.TicketInfoDtos
{
    public class CreateTicketInfoDto
    {
        public required Guid PosterId { get; set; }
        public required TicketType TicketType { get; set; }
        public required decimal Price { get; set; }
        public required int TotalCount { get; set; }
    }
}
