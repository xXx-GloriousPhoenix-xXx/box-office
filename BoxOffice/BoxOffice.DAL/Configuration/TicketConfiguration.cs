using BoxOffice.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> e)
        {
            e.HasOne(t => t.TicketInfo)
            .WithMany(ti => ti.Tickets)
            .HasForeignKey(t => t.TicketInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.Customer)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

            e.Property(e => e.TicketState)
                .HasConversion<string>()
                .HasMaxLength(16);
        }
    }
}
