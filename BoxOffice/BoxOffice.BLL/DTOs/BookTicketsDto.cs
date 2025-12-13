namespace BoxOffice.BLL.DTO
{
    public class BookTicketsDto
    {
        public Guid CustomerId { get; set; }
        public Guid PosterId { get; set; }
        public List<string> SeatNumbers { get; set; } = [];
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
    }
}