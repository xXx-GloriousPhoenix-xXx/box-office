using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.TicketInfoDtos
{
    public class CreateTicketInfoAdditionDto
    {
        public TicketType TicketType { get; set; }
        public decimal Price { get; set; }
        public int TotalCount { get; set; }
    }
}
