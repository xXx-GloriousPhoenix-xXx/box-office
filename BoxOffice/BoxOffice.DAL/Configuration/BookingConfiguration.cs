using BoxOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> e)
        {
            // ===== Booking — Ticket (1:1) =====
            e.HasOne(b => b.Ticket)
            .WithOne(t => t.Booking)
            .HasForeignKey<Ticket>(t => t.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
            // Якщо бронювання - BookingId записується як null

            e.Property(e => e.State)
                .HasConversion<string>()
                .HasMaxLength(16);
            // Запис типів у рядок
        }
    }
}
