using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController(ITicketService ticketService, ILogger<TicketsController> logger) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;
        private readonly ILogger<TicketsController> _logger = logger;

        /// <summary>
        /// Отримати інформацію про квиток
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TicketDetailsDto>> GetTicket(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var ticket = await _ticketService.GetTicketDetailsAsync(id, ct);
                if (ticket == null)
                    return NotFound(new { message = $"Ticket with ID {id} not found" });

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ticket with ID {TicketId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати доступні квитки для вистави
        /// </summary>
        [HttpGet("poster/{posterId:guid}/available")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAvailableTickets(
            Guid posterId,
            CancellationToken ct = default)
        {
            try
            {
                var tickets = await _ticketService.GetAvailableTicketsAsync(posterId, ct);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available tickets for poster {PosterId}", posterId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати квитки клієнта
        /// </summary>
        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetCustomerTickets(
            Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var tickets = await _ticketService.GetCustomerTicketsAsync(customerId, ct);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tickets for customer {CustomerId}", customerId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Забронювати квитки
        /// </summary>
        [HttpPost("book")]
        public async Task<ActionResult<BookingResultDto>> BookTickets(
            [FromBody] BookTicketsDto request,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _ticketService.BookTicketsAsync(request, ct);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking tickets");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Продати квиток
        /// </summary>
        [HttpPost("sell")]
        public async Task<ActionResult<SaleResultDto>> SellTicket(
            [FromBody] SellTicketDto request,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _ticketService.SellTicketAsync(request, ct);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selling ticket");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Конвертувати бронювання у продаж
        /// </summary>
        [HttpPost("booking/{bookingToken}/sell")]
        public async Task<ActionResult> ConvertBookingToSale(
            string bookingToken,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _ticketService.ConvertBookingToSaleAsync(bookingToken, ct);
                if (!result)
                    return NotFound(new { message = $"Active booking with token {bookingToken} not found" });

                return Ok(new { message = "Booking successfully converted to sale" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting booking {BookingToken} to sale", bookingToken);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Скасувати бронювання
        /// </summary>
        [HttpDelete("booking/{bookingToken}")]
        public async Task<ActionResult> CancelBooking(
            string bookingToken,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _ticketService.CancelBookingAsync(bookingToken, ct);
                if (!result)
                    return NotFound(new { message = $"Active booking with token {bookingToken} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling booking {BookingToken}", bookingToken);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Оновити статус квитка
        /// </summary>
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<TicketDto>> UpdateTicketStatus(
            Guid id,
            [FromBody] UpdateTicketStatusRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Эту логику нужно добавить в TicketService
                // Для простоты пока возвращаем заглушку
                return Ok(new { message = $"Ticket {id} status updated to {request.State}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket status for ticket {TicketId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    public class UpdateTicketStatusRequest
    {
        public required string State { get; set; }
    }
}