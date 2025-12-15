using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Models.Entities
{
    [Table("posters")]
    public class Poster : BaseEntity
    {
        [Column("performance_name")]
        public required string Name { get; set; }

        [Column("performance_description")]
        public required string Description { get; set; }

        [Column("release_date")]
        public DateOnly Date { get; set; }

        [Column("performance_venue")]
        public required string Venue { get; set; }

        [Column("performance_duration")]
        public int Duration { get; set; }

        [Column("performance_author")]
        public Guid AuthorId { get; set; }

        [Column(nameof(AuthorId))]
        public required Author Author { get; set; }

        public virtual ICollection<Genre> Genres { get; set; } = [];

        public virtual ICollection<TicketInfo> TicketInfos { get; set; } = [];
    }
}
