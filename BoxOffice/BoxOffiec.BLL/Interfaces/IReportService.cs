using BoxOffice.BLL.DTO;

namespace BoxOffice.BLL.Interfaces
{
    public interface IReportService
    {
        Task<RevenueReportDto> GetRevenueReportAsync(DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
        Task<IEnumerable<PosterSalesDto>> GetTopSellingPostersAsync(int topCount = 10, CancellationToken ct = default);
        Task<OccupancyReportDto> GetOccupancyReportAsync(Guid posterId, CancellationToken ct = default);
    }
}