using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.TicketDtos
{
    public class GetTicketWithTransactionsDto : GetTicketDto
    {
        public required ICollection<GetTransactionAdditionDto> Transactions { get; set; }
    }
}
