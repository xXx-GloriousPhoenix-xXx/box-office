using BoxOffice.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class TicketInfoConfiguration : IEntityTypeConfiguration<TicketInfo>
    {
        public void Configure(EntityTypeBuilder<TicketInfo> e)
        {
            e.HasOne(ti => ti.Poster)
            .WithMany(p => p.TicketInfos)
            .HasForeignKey(ti => ti.PosterId)
            .OnDelete(DeleteBehavior.Cascade);

            e.Property(e => e.Price)
                .HasPrecision(6, 2)
                .IsRequired();

            e.Property(e => e.TicketType)
                .HasConversion<string>()
                .HasMaxLength(16);
        }
    }
}
