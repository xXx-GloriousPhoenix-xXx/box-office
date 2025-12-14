using BoxOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> e)
        {
            // ===== Ticket — Transaction (1:N) =====
            e.HasOne(tr => tr.Ticket)
            .WithMany(t => t.Transactions)
            .HasForeignKey(tr => tr.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
            // Якщо видалити білет - транзації видаляються

            // ===== Customer — Transaction (1:N) =====
            e.HasOne(tr => tr.Customer)
            .WithMany(c => c.Transactions)
            .HasForeignKey(tr => tr.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
            // Якщо видалити клієнта - транзації видаляються

            e.Property(t => t.Amount)
                .HasPrecision(7, 2);
            // Максимум 99_999.99

            e.Property(e => e.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(16);
            e.Property(e => e.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(16);
            // Запис типів у рядок
        }
    }
}
