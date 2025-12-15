using BoxOffice.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.BLL.DTOs.TicketDtos
{
    public class GetTicketDto
    {
        public Guid Id { get; set; }
        public required string SeatNumber { get; set; }
        public TicketState TicketState { get; set; }
        public DateOnly? SoldDate { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? BookingId { get; set; }
    }
}
