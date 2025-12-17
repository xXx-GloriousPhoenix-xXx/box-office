using BoxOffice.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> e)
        {
            e.HasOne(tr => tr.Ticket)
            .WithMany(t => t.Transactions)
            .HasForeignKey(tr => tr.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(tr => tr.Customer)
            .WithMany(c => c.Transactions)
            .HasForeignKey(tr => tr.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            e.Property(t => t.Amount)
                .HasPrecision(8, 2);

            e.Property(e => e.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(16);
            e.Property(e => e.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(16);
        }
    }
}
