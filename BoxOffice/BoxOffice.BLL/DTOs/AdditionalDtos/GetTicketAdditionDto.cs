using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTOs.AdditionalDtos
{
    public class GetTicketAdditionDto
    {
        public Guid Id { get; set; }
        public required string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public TicketType Type { get; set; }
        public TicketState State { get; set; }
    }
}
