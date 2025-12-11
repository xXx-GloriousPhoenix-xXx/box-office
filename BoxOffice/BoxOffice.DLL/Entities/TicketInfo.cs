using BoxOffice.DLL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("tickets_info")]
    public class TicketInfo : BaseEntity
    {
        [Column("poster_id")]
        public Guid PosterId { get; set; }

        [Column("ticket_type")]
        public TicketType TicketType { get; set; }

        [Column("ticket_price")]
        public decimal Price { get; set; }

        [Column("total_tickets")]
        public int TotalTickets { get; set; }

        [Column("available_tickets")]
        public int AvailableTickets { get; set; }

        [Column("sold_tickets")]
        public int SoldTickets { get; set; }

        [Column("booked_tickets")]
        public int BookedTickets { get; set; }

        [ForeignKey(nameof(PosterId))]
        public Poster? Poster { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
