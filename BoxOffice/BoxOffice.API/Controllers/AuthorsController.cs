using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController(IAuthorService service) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAuthorDto>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpGet("{page}/{itemsPerPage}")]
        public async Task<ActionResult<PagedResponse<GetAuthorDto>>> GetAllAsync(int page, int itemsPerPage, CancellationToken ct)
        {
            var result = await service.GetAllAsync(page, itemsPerPage, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetAuthorDto>> AddAsync([FromForm] CreateAuthorDto dto, CancellationToken ct)
        {
            var result = await service.AddAsync(dto, ct);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetAuthorDto>> UpdateAsync(Guid id, [FromForm] UpdateAuthorDto dto, CancellationToken ct)
        {
            var result = await service.UpdateAsync(id, dto, ct);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);
            return Ok();
        }

        [HttpGet("{id}/posters")]
        public async Task<ActionResult<GetAuthorWithPostersDto>> GetAuthorPostersAsync(Guid id, CancellationToken ct)
        {
            var result = await service.GetAuthorPostersAsync(id, ct);
            return Ok(result);
        }
    }
}
