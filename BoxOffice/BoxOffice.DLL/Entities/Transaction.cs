using BoxOffice.DLL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("transactions")]
    public class Transaction : BaseEntity
    {
        [Column("ticket_id")]
        public Guid TicketId { get; set; }

        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Column("transaction_type")]
        public TransactionType TransactionType { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; } = "Card";

        [Column("payment_reference")]
        public string? PaymentReference { get; set; }

        [ForeignKey(nameof(TicketId))]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }
    }
}
