using BoxOffice.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BoxOffice.DAL.Configuration
{
    public class PosterConfiguration : IEntityTypeConfiguration<Poster>
    {
        public void Configure(EntityTypeBuilder<Poster> e)
        {
            // ===== Author — Poster (1:N) =====
            e.HasOne(p => p.Author)
            .WithMany(a => a.Posters)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
            // Не можна видалити автора, якщо у нього є афіши

            // ===== Poster — Genre (M:N) =====
            e.HasMany(p => p.Genres)
            .WithMany(g => g.Posters)
            .UsingEntity<Dictionary<string, object>>(
                "poster_genres",
                j => j
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("genre_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Poster>()
                    .WithMany()
                    .HasForeignKey("poster_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("poster_id", "genre_id");
                });
            // Якщо видаляється афіша - видаляються зв'язки з жанром
            // Якщо видаляється жанр - видаляються зв'язки з афішами
        }
    }
}
