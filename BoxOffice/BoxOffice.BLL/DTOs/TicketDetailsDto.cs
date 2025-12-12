namespace BoxOffice.BLL.DTO
{
    public class TicketDetailsDto : TicketDto
    {
        public PosterDto Poster { get; set; } = null!;
        public CustomerDto? Customer { get; set; }
        public BookingDto? Booking { get; set; }
        public DateTime? SoldDate { get; set; }
    }
}