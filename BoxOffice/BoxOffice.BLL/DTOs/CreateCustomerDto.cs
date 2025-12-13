namespace BoxOffice.BLL.DTO
{
    public class CreateCustomerDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}