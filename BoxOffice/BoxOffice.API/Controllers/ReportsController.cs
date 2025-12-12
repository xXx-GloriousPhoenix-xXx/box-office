using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController(IReportService reportService, ILogger<ReportsController> logger) : ControllerBase
    {
        private readonly IReportService _reportService = reportService;
        private readonly ILogger<ReportsController> _logger = logger;

        /// <summary>
        /// Отримати звіт про прибуток
        /// </summary>
        [HttpGet("revenue")]
        public async Task<ActionResult<RevenueReportDto>> GetRevenueReport(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            CancellationToken ct = default)
        {
            try
            {
                var report = await _reportService.GetRevenueReportAsync(startDate, endDate, ct);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating revenue report");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати звіт про зайнятість місць
        /// </summary>
        [HttpGet("poster/{posterId:guid}/occupancy")]
        public async Task<ActionResult<OccupancyReportDto>> GetOccupancyReport(
            Guid posterId,
            CancellationToken ct = default)
        {
            try
            {
                var report = await _reportService.GetOccupancyReportAsync(posterId, ct);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting occupancy report for poster {PosterId}", posterId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати топ вистав за продажом
        /// </summary>
        [HttpGet("posters/top-selling")]
        public async Task<ActionResult<IEnumerable<PosterSalesDto>>> GetTopSellingPosters(
            [FromQuery] int topCount = 10,
            CancellationToken ct = default)
        {
            try
            {
                var posters = await _reportService.GetTopSellingPostersAsync(topCount, ct);
                return Ok(posters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top selling posters");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Отримати звіт про активність клієнта
        /// </summary>
        [HttpGet("customer/{customerId:guid}/activity")]
        public async Task<ActionResult<CustomerActivityReportDto>> GetCustomerActivityReport(
            Guid customerId,
            CancellationToken ct = default)
        {
            try
            {
                var report = await _reportService.GetCustomerActivityReportAsync(customerId, ct);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer activity report for customer {CustomerId}", customerId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}