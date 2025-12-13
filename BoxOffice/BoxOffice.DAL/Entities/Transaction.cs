using BoxOffice.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Entities
{
    [Table("transactions")]
    public class Transaction : BaseEntity
    {
        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("type")]
        public TransactionType TransactionType { get; set; }

        [Column("customer")]
        public Guid CustomerId { get; set; }

        [Column("ticket")]
        public Guid TicketId { get; set; }

        [Column(nameof(CustomerId))]
        public required Customer Customer { get; set; }

        [Column(nameof(TicketId))]
        public required Ticket Ticket { get; set; }
    }
}
