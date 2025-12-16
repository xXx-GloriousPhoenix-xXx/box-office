using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.GenreDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController(IGenreService service) : ControllerBase
    {
        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<PagedResponse<GetGenreDto>>> GetAllAsync(CancellationToken ct, int page = 1, int itemsPerPage = 10)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetGenreDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetGenreDto>> AddAsync([FromForm] CreateGenreDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetGenreDto>> UpdateAsync(Guid id, [FromForm] UpdateGenreDto dto, CancellationToken ct)
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
    }
}
