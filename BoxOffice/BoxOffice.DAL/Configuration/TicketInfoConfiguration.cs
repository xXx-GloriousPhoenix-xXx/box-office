using BoxOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxOffice.DAL.Configuration
{
    public class TicketInfoConfiguration : IEntityTypeConfiguration<TicketInfo>
    {
        public void Configure(EntityTypeBuilder<TicketInfo> e)
        {
            // ===== Poster — TicketInfo (1:N) =====
            e.HasOne(ti => ti.Poster)
            .WithMany(p => p.TicketInfos)
            .HasForeignKey(ti => ti.PosterId)
            .OnDelete(DeleteBehavior.Cascade);
            // Якщо видаляється афіша - видаляються зв'язані з нею види квитків

            e.Property(e => e.Price)
                .HasPrecision(5, 2)
                .IsRequired();
            // Максимум 999.99

            e.Property(e => e.TicketType)
                .HasConversion<string>()
                .HasMaxLength(16);
            // Запис типів у рядок
        }
    }
}
