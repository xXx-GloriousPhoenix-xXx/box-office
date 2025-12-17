using BoxOffice.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> e)
        {
            e.HasOne(b => b.Ticket)
            .WithOne(t => t.Booking)
            .HasForeignKey<Ticket>(t => t.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

            e.Property(e => e.State)
                .HasConversion<string>()
                .HasMaxLength(16);
        }
    }
}
