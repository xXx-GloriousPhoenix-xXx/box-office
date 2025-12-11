using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class SeatStatusDto
    {
        public string SeatNumber { get; set; } = string.Empty;
        public TicketState Status { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
    }
}