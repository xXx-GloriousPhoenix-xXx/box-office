using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("customers")]
    public class Customer : BaseEntity
    {
        [Column("customer_name")]
        public required string Name { get; set; }
        
        [Column("customer_email")]
        public required string Email { get; set; }

        [Column("customer_phone")]
        public string? PhoneNumber { get; set; }

        public ICollection<Ticket> PurchasedTickets { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
