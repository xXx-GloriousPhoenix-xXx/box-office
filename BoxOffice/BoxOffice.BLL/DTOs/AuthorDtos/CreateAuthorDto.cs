using System.ComponentModel.DataAnnotations;

namespace BoxOffice.BLL.DTOs.AuthorDtos
{
    public class CreateAuthorDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
