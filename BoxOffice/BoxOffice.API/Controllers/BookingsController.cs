using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.AdditionalDtos.OperationDtos;
using BoxOffice.BLL.DTOs.BookingDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController(IBookingService service) : ControllerBase
    {
        [HttpGet("{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetBookingDto>>> GetAllAsync(CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookingDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost("book/{ticketId}")]
        public async Task<ActionResult<GetBookingDto>> BookAsync(Guid ticketId, [FromForm] BookingDto dto, CancellationToken ct)
        {
            var result = await service.BookAsync(ticketId, dto, ct);
            return Ok(result);
        }

        [HttpPost("cancel/{ticketId}")]
        public async Task<IActionResult> CancelAsync(Guid ticketId, [FromForm] CancelBookingDto dto, CancellationToken ct)
        {
            await service.CancelBookingAsync(ticketId, dto, ct);
            return Ok();
        }
    }
}
