using BoxOffice.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Entities
{
    [Table("tickets")]
    public class Ticket : BaseEntity
    {
        [Column("ticket_info")]
        public Guid TicketInfoId { get; set; }

        [Column("seat_number")]
        public required string SeatNumber { get; set; }

        [Column("customer")]
        public Guid? CustomerId { get; set; }   

        [Column("state")]
        public TicketState TicketState { get; set; }

        [Column("booking")]
        public Guid? BookingId { get; set; }

        [Column("sold_date")]
        public DateOnly? SoldDate { get; set; }

        [Column(nameof(BookingId))]
        public Booking? Booking { get; set; }

        [Column(nameof(TicketInfoId))]
        public required TicketInfo TicketInfo { get; set; }

        [Column(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = [];
    }
}
