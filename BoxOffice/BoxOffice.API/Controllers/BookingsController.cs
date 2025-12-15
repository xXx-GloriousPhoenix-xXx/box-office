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
        public async Task<ActionResult<PagedResponse<GetBookingDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookingDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetBookingDto>> AddAsync([FromForm] CreateBookingDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("book/{ticketId}")]
        public async Task<ActionResult<GetBookingDto>> BookAsync(Guid ticketId)
        {
            var result = service.BookAsync(ticketId);
            return Ok(result);
        }

        [HttpPost("cancel/{ticketId}")]
        public async Task<IActionResult> CancelAsync(Guid ticketId)
        {
            await service.CancelBookingAsync(ticketId);
            return Ok();
        }
    }
}
