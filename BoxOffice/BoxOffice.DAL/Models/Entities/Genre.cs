using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Models.Entities
{
    [Table("genres")]
    public class Genre : BaseEntity
    {
        [Column("name")]
        public required string Name { get; set; }

        public virtual ICollection<Poster> Posters { get; set; } = [];
    }
}
