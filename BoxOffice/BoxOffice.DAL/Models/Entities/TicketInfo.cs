using BoxOffice.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Models.Entities
{
    [Table("ticket_infos")]
    public class TicketInfo : BaseEntity
    {
        [Column("poster")]
        public Guid PosterId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("total_tickets")]
        public int TotalCount { get; set; }

        [Column("available_tickets")]
        public int AvailableCount { get; set; }

        [Column("sold_tickets")]
        public int SoldCount { get; set; }

        [Column("booked_tickets")]
        public int BookedCount { get; set; }

        [Column("type")]
        public TicketType TicketType { get; set; }

        [Column(nameof(PosterId))]
        public Poster? Poster { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = [];
    }
}
