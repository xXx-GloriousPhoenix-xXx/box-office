using BoxOffice.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Entities
{
    [Table("posters")]
    public class Poster : BaseEntity
    {
        [Column("author_id")]
        public Guid AuthorId { get; set; }

        [Column("poster_name")]
        public required string Name { get; set; }

        [Column("poster_description")]
        public required string Description { get; set; }

        [Column("poster_genres")]
        public List<PosterGenre> Genres { get; set; } = [];

        [Column("release_date")]
        public DateOnly ReleaseDate { get; set; }

        [Column("performance_venue")]
        public required string Venue { get; set; }

        [Column("performance_duration")]
        public int DurationMinutes { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }

        public ICollection<TicketInfo> TicketInfos { get; set; } = [];
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
