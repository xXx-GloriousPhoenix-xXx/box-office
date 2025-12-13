using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Entities
{
    [Table("customers")]
    public class Customer : BaseEntity
    {
        [Column("name")]
        public required string Name { get; set; }

        [Column("email")]
        public required string Email { get; set; }

        [Column("phone_number")]
        public required string Phone { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = [];

        public virtual ICollection<Ticket> Tickets { get; set; } = [];
    }
}
