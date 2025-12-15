using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/ticket-infos")]
    public class TicketInfosController(ITicketInfoService service) : ControllerBase
    {
        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<PagedResponse<GetTicketInfoDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetTicketInfoDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetTicketInfoDto>> AddAsync([FromForm] CreateTicketInfoDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetTicketInfoDto>> UpdateAsync(Guid id, [FromForm] UpdateTicketInfoDto dto)
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
    }
}
