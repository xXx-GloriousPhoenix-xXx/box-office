using BoxOffice.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Entities
{
    [Table("bookings")]
    public class Booking : BaseEntity
    {
        [Column("booked_at")]
        public DateOnly BookedAt { get; set; }

        [Column("expires_at")]
        public DateOnly ExpiresAt { get; set; }

        [Column("token")]
        public required string BookingToken { get; set; }

        [Column("state")]
        public BookingState State { get; set; }

        public required virtual Ticket Ticket { get; set; }
    }
}
