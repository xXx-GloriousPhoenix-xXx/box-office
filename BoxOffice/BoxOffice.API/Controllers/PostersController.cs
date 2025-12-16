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
        public async Task<ActionResult<PagedResponse<GetPosterWithTicketInfosDto>>> GetAllAsync(int page, int itemsPerPage, [FromForm] SearchPosterDto? dto)
        {
            var result = await service.GetAllAsync(dto, page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetPosterWithTicketInfosDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetPosterWithTicketInfosDto>> AddAsync([FromForm] CreatePosterDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetPosterDto>> UpdateAsync(Guid id, [FromForm] UpdatePosterDto dto)
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

        [HttpGet("{id:guid}/tickets/{state}")]
        public async Task<ActionResult<GetPosterWithTicketsDto>> GetPosterTicketsAsync(Guid id, TicketState state)
        {
            var result = await service.GetPosterTicketsAsync(id, state);
            return Ok(result);
        }

        [HttpGet("{id:guid}/stats")]
        public async Task<ActionResult<GetPosterStatsDto>> GetPosterStatisticsAsync(Guid id)
        {
            var result = await service.GetPosterStatisticsAsync(id);
            return Ok(result);
        }
    }
}
