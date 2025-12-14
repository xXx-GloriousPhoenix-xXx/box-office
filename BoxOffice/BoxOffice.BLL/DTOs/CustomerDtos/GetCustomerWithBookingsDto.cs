using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.CustomerDtos
{
    public class GetCustomerWithBookingsDto
    {
        public ICollection<GetBookingAdditionDto> Bookings { get; set; } = [];
    }
}
