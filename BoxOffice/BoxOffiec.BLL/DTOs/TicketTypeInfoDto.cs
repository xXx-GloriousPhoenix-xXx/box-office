using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class TicketTypeInfoDto
    {
        public Guid Id { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
        public int AvailableTickets { get; set; }
        public int TotalTickets { get; set; }
    }
}