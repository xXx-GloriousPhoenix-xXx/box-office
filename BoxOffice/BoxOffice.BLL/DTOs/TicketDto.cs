using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public TicketState State { get; set; }
        public decimal Price { get; set; }
        public TicketType Type { get; set; }
        public DateTime? BookedUntil { get; set; }
    }
}