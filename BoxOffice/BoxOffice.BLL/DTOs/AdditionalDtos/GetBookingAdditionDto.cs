using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.AdditionalDtos
{
    public class GetBookingAdditionDto
    {
        public Guid Id { get; set; }
        public DateOnly BookedAt { get; set; }
        public DateOnly ExpiresAt { get; set; }
        public required string BookingToken { get; set; }
        public BookingState State { get; set; }
    }
}
