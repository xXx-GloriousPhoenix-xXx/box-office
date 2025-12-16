using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController(ITransactionService service) : ControllerBase
    {
        [HttpGet("{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetTransactionDto>>> GetAllAsync(CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTransactionDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetTransactionDto>> AddAsync([FromForm] CreateTransactionDto dto, CancellationToken ct)
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

        [HttpPost("customer/{id}/{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetTransactionDto>>> GetCustomerTransactionsAsync(Guid id, CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetCustomerTransactionsAsync(id, page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpPost("ticket/{id}")]
        public async Task<ActionResult<GetTransactionDto>> GetTicketTransactionsAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetTicketTransactionsAsync(id, ct);
            return Ok(result);
        }

    }
}
