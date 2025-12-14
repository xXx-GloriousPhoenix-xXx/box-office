using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController(ICustomerService service) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCustomerDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<IEnumerable<GetCustomerDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetCustomerDto>> AddAsync([FromForm] CreateCustomerDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetCustomerDto>> UpdateAsync(Guid id, [FromForm] UpdateCustomerDto dto)
        {
            var result = await service.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id:guid}/tickets")]
        public async Task<ActionResult<GetCustomerWithTicketsDto>> GetCustomerTicketsAsync(Guid id)
        {
            var result = await service.GetCustomerTicketsAsync(id);
            return Ok(result);
        }

        [HttpGet("{id:guid}/bookings")]
        public async Task<ActionResult<GetCustomerWithBookingsDto>> GetCustomerBookingsAsync(Guid id)
        {
            var result = await service.GetCustomerBookingsAsync(id);
            return Ok(result);
        }
    }
}
