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
        public async Task<ActionResult<PagedResponse<GetTransactionDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTransactionDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetTransactionDto>> AddAsync([FromForm] CreateTransactionDto dto)
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

        [HttpPost("customer/{id}/{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetTransactionDto>>> GetCustomerTransactionsAsync(Guid id, int page, int itemsPerPage)
        {
            var result = await service.GetCustomerTransactionsAsync(page, itemsPerPage, id);
            return Ok(result);
        }

        [HttpPost("ticket/{id}")]
        public async Task<ActionResult<GetTransactionDto>> GetTicketTransactionsAsync(Guid id)
        {
            var result = await service.GetTicketTransactionsAsync(id);
            return Ok(result);
        }

    }
}
