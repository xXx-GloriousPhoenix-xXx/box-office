using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Models.Entities
{
    [Table("authors")]
    public class Author : BaseEntity
    {
        [Column("name")]
        public required string Name { get; set; }

        public virtual ICollection<Poster> Posters { get; set; } = [];
    }
}
