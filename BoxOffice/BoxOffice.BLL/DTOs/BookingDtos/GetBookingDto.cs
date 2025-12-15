using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.BookingDtos
{
    public class GetBookingDto
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public DateOnly BookedAt { get; set; }
        public DateOnly ExpiresAt { get; set; }
        public required string BookingToken { get; set; }
        public BookingState State { get; set; }
    }
}
