using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostersController(IPosterService posterService, ILogger<PostersController> logger) : ControllerBase
    {
        private readonly IPosterService _posterService = posterService;
        private readonly ILogger<PostersController> _logger = logger;

        /// <summary>
        /// Отримати всі вистави
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PosterDto>>> GetAllPosters(
            CancellationToken ct = default)
        {
            try
            {
                var posters = await _posterService.SearchPostersAsync(new SearchPostersDto(), ct);
                return Ok(posters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all posters");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Пошук вистав
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PosterDto>>> SearchPosters(
            [FromQuery] string? authorName = null,
            [FromQuery] string? title = null,
            [FromQuery] string? genre = null,
            [FromQuery] DateOnly? dateFrom = null,
            [FromQuery] DateOnly? dateTo = null,
            CancellationToken ct = default)
        {
            try
            {
                PosterGenre? genreEnum = null;
                if (!string.IsNullOrEmpty(genre) && Enum.TryParse<PosterGenre>(genre, true, out var parsedGenre))
                {
                    genreEnum = parsedGenre;
                }

                var searchDto = new SearchPostersDto
                {
                    AuthorName = authorName,
                    Title = title,
                    Genre = genreEnum,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };

                var posters = await _posterService.SearchPostersAsync(searchDto, ct);
                return Ok(posters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching posters");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати інформацію про виставу
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PosterDetailsDto>> GetPoster(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var poster = await _posterService.GetPosterDetailsAsync(id, ct);
                if (poster == null)
                    return NotFound(new { message = $"Poster with ID {id} not found" });

                return Ok(poster);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting poster with ID {PosterId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати найближчі вистави
        /// </summary>
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<PosterDto>>> GetUpcomingPosters(
            [FromQuery] int daysAhead = 30,
            CancellationToken ct = default)
        {
            try
            {
                var posters = await _posterService.GetUpcomingPostersAsync(daysAhead, ct);
                return Ok(posters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting upcoming posters");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Створити нову виставу
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PosterDto>> CreatePoster(
            [FromBody] CreatePosterDto request,
            CancellationToken ct = default)
        {
            try
            {
                var poster = await _posterService.CreatePosterAsync(request, ct);
                return CreatedAtAction(nameof(GetPoster), new { id = poster.Id }, poster);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating poster");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Оновити інформацію про виставу
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PosterDto>> UpdatePoster(
            Guid id,
            [FromBody] UpdatePosterDto request,
            CancellationToken ct = default)
        {
            try
            {
                var poster = await _posterService.UpdatePosterAsync(id, request, ct);
                return Ok(poster);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating poster with ID {PosterId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Видалити виставу
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeletePoster(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _posterService.DeletePosterAsync(id, ct);
                if (!result)
                    return NotFound(new { message = $"Poster with ID {id} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting poster with ID {PosterId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}