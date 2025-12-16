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
        public async Task<ActionResult<PagedResponse<GetTicketDto>>> GetAllAsync(CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetTicketDto>> AddAsync([FromForm] CreateTicketDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpPost("{id}/purchase")]
        public async Task<ActionResult<GetTicketDto>> PurchaseAsync(Guid id, CancellationToken ct)
        {
            var result = await service.PurchaseAsync(id, ct);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<GetTicketDto>> CancelPurchaseAsync(Guid id, CancellationToken ct)
        {
            var result = await service.CancelPurchaseAsync(id, ct);
            return Ok(result);
        }
    }
}
