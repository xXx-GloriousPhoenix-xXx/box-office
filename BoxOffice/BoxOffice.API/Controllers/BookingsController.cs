using BoxOffice.BLL.DTOs.AdditionalDtos;
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

        [HttpPost]
        public async Task<ActionResult<GetBookingDto>> AddAsync([FromForm] CreateBookingDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);
            return Ok();
        }

        [HttpPost("book/{ticketId}")]
        public async Task<ActionResult<GetBookingDto>> BookAsync(Guid ticketId, CancellationToken ct)
        {
            var result = service.BookAsync(ticketId, ct);
            return Ok(result);
        }

        [HttpPost("cancel/{ticketId}")]
        public async Task<IActionResult> CancelAsync(Guid ticketId, CancellationToken ct)
        {
            await service.CancelBookingAsync(ticketId, ct);
            return Ok();
        }
    }
}
