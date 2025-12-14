using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.CustomerDtos
{
    public class GetCustomerWithTicketsDto : GetCustomerDto
    {
        public ICollection<GetTicketAdditionDto> Tickets { get; set; } = [];
    }
}
