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
            e.HasOne(p => p.Author)
            .WithMany(a => a.Posters)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

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
        }
    }
}
