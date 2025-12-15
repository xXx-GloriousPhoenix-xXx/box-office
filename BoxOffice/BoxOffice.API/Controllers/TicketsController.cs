using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController(ITicketService service) : ControllerBase
    {
        [HttpGet("{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetTicketDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetTicketDto>> AddAsync([FromForm] CreateTicketDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPost("{id}/purchase")]
        public async Task<ActionResult<GetTicketDto>> PurchaseAsync(Guid id)
        {
            var result = await service.PurchaseAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<GetTicketDto>> CancelPurchaseAsync(Guid id)
        {
            var result = await service.CancelPurchaseAsync(id);
            return Ok(result);
        }
    }
}
