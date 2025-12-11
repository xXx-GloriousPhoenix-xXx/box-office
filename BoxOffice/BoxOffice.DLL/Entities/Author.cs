using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DLL.Entities
{
    [Table("authors")]
    public class Author : BaseEntity
    {
        [Column("author_name")]
        public required string Name { get; set; }

        public virtual ICollection<Poster> Posters { get; set; } = [];
    }
}
