namespace BoxOffice.BLL.DTOs.CustomerDtos
{
    public class GetCustomerDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
