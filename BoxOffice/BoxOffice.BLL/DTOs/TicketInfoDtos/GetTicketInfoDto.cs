using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.TicketInfoDtos
{
    public class GetTicketInfoDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
        public int BookedCount { get; set; }
        public TicketType TicketType { get; set; }
    }
}
