using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/posters")]
    public class PostersController(IPosterService service) : ControllerBase
    {
        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<PagedResponse<GetPosterDto>>> GetAllAsync(CancellationToken ct, [FromQuery] SearchPosterDto? dto, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(dto, page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetPosterWithTicketInfosDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetPosterWithTicketInfosDto>> AddAsync([FromBody] CreatePosterDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetPosterDto>> UpdateAsync(Guid id, [FromForm] UpdatePosterDto dto, CancellationToken ct)
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

        [HttpGet("{id:guid}/tickets/{state}")]
        public async Task<ActionResult<GetPosterWithTicketsDto>> GetPosterTicketsAsync(Guid id, TicketState state, CancellationToken ct)
        {
            var result = await service.GetPosterTicketsAsync(id, state, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/stats")]
        public async Task<ActionResult<GetPosterStatsDto>> GetPosterStatisticsAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetPosterStatisticsAsync(id, ct);
            return Ok(result);
        }
    }
}
