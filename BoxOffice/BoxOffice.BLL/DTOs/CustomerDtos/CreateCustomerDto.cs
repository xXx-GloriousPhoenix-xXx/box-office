using System.ComponentModel.DataAnnotations;

namespace BoxOffice.BLL.DTOs.CustomerDtos
{
    public class CreateCustomerDto
    {
        [Required]
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
    }
}
