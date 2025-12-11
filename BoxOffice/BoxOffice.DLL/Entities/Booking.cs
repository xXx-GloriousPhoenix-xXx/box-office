using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("bookings")]
    public class Booking : BaseEntity
    {
        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [Column("booking_date")]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("booking_token")]
        public required string BookingToken { get; set; }

        [Column("status")]
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = [];
    }

    public enum BookingStatus
    {
        Active,
        Completed,
        Cancelled,
        Expired
    }
}
