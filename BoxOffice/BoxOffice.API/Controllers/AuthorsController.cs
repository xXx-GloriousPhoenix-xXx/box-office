using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController(IAuthorService service) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetAuthorDto>> GetByIdAsync(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{page:int}/{itemsPerPage:int}")]
        public async Task<ActionResult<IEnumerable<GetAuthorDto>>> GetAllAsync(int page, int itemsPerPage)
        {
            var result = await service.GetAllAsync(page, itemsPerPage);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetAuthorDto>> AddAsync([FromForm] CreateAuthorDto dto)
        {
            var result = await service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetAuthorDto>> UpdateAsync(Guid id, [FromForm] UpdateAuthorDto dto)
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

        [HttpGet("{id:guid}/posters")]
        public async Task<ActionResult<GetAuthorWithPostersDto>> GetAuthorPostersAsync(Guid id)
        {
            var result = await service.GetAuthorPostersAsync(id);
            return Ok(result);
        }
    }
}
