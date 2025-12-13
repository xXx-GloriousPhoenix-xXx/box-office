using BoxOffice.BLL.DTO;
using BoxOffice.BLL.DTOs;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger) : ControllerBase
    {
        private readonly IAuthorService _authorService = authorService;
        private readonly ILogger<AuthorsController> _logger = logger;

        /// <summary>
        /// Отримати всіх авторів
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(
            CancellationToken ct = default)
        {
            try
            {
                var authors = await _authorService.GetAllAuthorsAsync(ct);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all authors");
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Створити нового автора
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(
            [FromBody] CreateAuthorDto request,
            CancellationToken ct = default)
        {
            try
            {
                var createDto = new CreateAuthorDto
                {
                    Name = request.Name,
                };

                var author = await _authorService.CreateAuthorAsync(createDto, ct);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating author");
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Отримати інформацію про автора
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id, ct);
                if (author == null)
                    return NotFound(new { message = $"Author with ID {id} not found" });

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting author with ID {AuthorId}", id);
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Оновити інформацію про автора
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(
            Guid id,
            [FromBody] UpdateAuthorDto request,
            CancellationToken ct = default)
        {
            try
            {
                var updateDto = new UpdateAuthorDto
                {
                    Name = request.Name,
                };

                var author = await _authorService.UpdateAuthorAsync(id, updateDto, ct);
                return Ok(author);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating author with ID {AuthorId}", id);
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Видалити автора
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAuthor(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _authorService.DeleteAuthorAsync(id, ct);
                if (!result)
                    return NotFound(new { message = $"Author with ID {id} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting author with ID {AuthorId}", id);
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}