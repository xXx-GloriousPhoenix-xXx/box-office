using System.Text.Json.Serialization;

namespace BoxOffice.DAL.Models.Enums
{
    public enum BookingState
    {
        Active,
        Completed,
        Expired,
        Cancelled
    }
}
