using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class CreateTicketTypeInfoDto
    {
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
        public int TotalTickets { get; set; }
    }
}