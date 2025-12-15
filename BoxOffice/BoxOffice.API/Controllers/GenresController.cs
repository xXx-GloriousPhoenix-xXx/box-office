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
        public async Task<ActionResult<PagedResponse<GetGenreDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetGenreDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetGenreDto>> AddAsync([FromForm] CreateGenreDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<GetGenreDto>> UpdateAsync(Guid id, [FromForm] UpdateGenreDto dto)
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
