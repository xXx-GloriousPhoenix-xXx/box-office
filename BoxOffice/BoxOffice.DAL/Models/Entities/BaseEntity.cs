using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.DAL.Models.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
    }
}
