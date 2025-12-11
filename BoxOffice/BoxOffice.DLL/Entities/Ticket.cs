using BoxOffice.DLL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("ticket")]
    public class Ticket : BaseEntity
    {
        [Column("ticket_info_id")]
        public Guid TicketInfoId { get; set; }

        [Column("poster_id")]
        public Guid PosterId { get; set; }

        // A10, B15
        [Column("seat_number")]
        public required string SeatNumber { get; set; } 

        [Column("ticket_state")]
        public TicketState State { get; set; } = TicketState.Available;

        [Column("customer_id")]
        public Guid? CustomerId { get; set; }

        [Column("booking_id")]
        public Guid? BookingId { get; set; }

        [Column("booked_until")]
        public DateTime? BookedUntil { get; set; }

        [Column("sold_date")]
        public DateTime? SoldDate { get; set; }

        [Column("unique_booking_token")]
        public string? UniqueBookingToken { get; set; }

        [ForeignKey(nameof(TicketInfoId))]
        public virtual TicketInfo? TicketInfo { get; set; }

        [ForeignKey(nameof(PosterId))]
        public virtual Poster? Poster { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Booking? Booking { get; set; }
    }
}
