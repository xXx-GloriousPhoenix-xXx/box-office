using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController(IBookingService bookingService, ILogger<BookingsController> logger) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;
        private readonly ILogger<BookingsController> _logger = logger;

        /// <summary>
        /// Отримати інформацію про бронювання
        /// </summary>
        [HttpGet("{bookingToken}")]
        public async Task<ActionResult<BookingDetailsDto>> GetBooking(
            string bookingToken,
            CancellationToken ct = default)
        {
            try
            {
                var booking = await _bookingService.GetBookingDetailsAsync(bookingToken, ct);
                if (booking == null)
                    return NotFound(new { message = $"Booking with token {bookingToken} not found" });

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking {BookingToken}", bookingToken);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Перевірити валідність бронювання
        /// </summary>
        [HttpGet("{bookingToken}/validate")]
        public async Task<ActionResult> ValidateBooking(
            string bookingToken,
            CancellationToken ct = default)
        {
            try
            {
                var isValid = await _bookingService.ValidateBookingAsync(bookingToken, ct);
                if (!isValid)
                    return NotFound(new { message = $"Booking with token {bookingToken} not found or invalid" });

                return Ok(new
                {
                    isValid = true,
                    message = "Booking is valid and active"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating booking {BookingToken}", bookingToken);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Подовжити бронювання
        /// </summary>
        [HttpPut("{bookingToken}/extend")]
        public async Task<ActionResult> ExtendBooking(
            string bookingToken,
            [FromQuery] int additionalHours = 24,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _bookingService.ExtendBookingAsync(bookingToken, additionalHours, ct);
                if (!result)
                    return NotFound(new { message = $"Active booking with token {bookingToken} not found" });

                return Ok(new
                {
                    message = $"Booking extended by {additionalHours} hours"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extending booking {BookingToken}", bookingToken);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати бронювання клієнта
        /// </summary>
        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetCustomerBookings(
            Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var bookings = await _bookingService.GetCustomerBookingsAsync(customerId, ct);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookings for customer {CustomerId}", customerId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати активні бронювання
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetActiveBookings(
            CancellationToken ct = default)
        {
            try
            {
                var bookings = await _bookingService.GetActiveBookingsAsync(ct);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active bookings");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Скасувати прострочені бронювання
        /// </summary>
        [HttpDelete("expired")]
        public async Task<ActionResult> CancelExpiredBookings(
            CancellationToken ct = default)
        {
            try
            {
                var result = await _bookingService.CancelExpiredBookingsAsync(ct);
                if (!result)
                    return Ok(new { message = "No expired bookings to cancel" });

                return Ok(new { message = "Expired bookings successfully cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling expired bookings");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}