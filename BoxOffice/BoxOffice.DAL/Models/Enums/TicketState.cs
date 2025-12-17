using System.Text.Json.Serialization;

namespace BoxOffice.DAL.Models.Enums
{
    public enum TicketState
    {
        Available,
        Booked,
        Sold,
        Cancelled
    }
}
