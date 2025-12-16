using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController(ICustomerService service) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCustomerDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<PagedResponse<GetCustomerDto>>> GetAllAsync(CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetCustomerDto>> AddAsync([FromForm] CreateCustomerDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetCustomerDto>> UpdateAsync(Guid id, [FromForm] UpdateCustomerDto dto, CancellationToken ct)
        {
            var result = await service.UpdateAsync(id, dto, ct);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);
            return Ok();
        }

        [HttpGet("{id:guid}/tickets")]
        public async Task<ActionResult<GetCustomerWithTicketsDto>> GetCustomerTicketsAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetCustomerTicketsAsync(id, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/bookings")]
        public async Task<ActionResult<GetCustomerWithBookingsDto>> GetCustomerBookingsAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetCustomerBookingsAsync(id, ct);
            return Ok(result);
        }
    }
}
