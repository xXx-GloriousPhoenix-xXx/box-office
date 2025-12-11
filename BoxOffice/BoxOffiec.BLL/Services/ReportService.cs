using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services
{
    public class ReportService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReportService> logger) : IReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ReportService> _logger = logger;

        public async Task<RevenueReportDto> GetRevenueReportAsync(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken ct = default)
        {
            var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
            var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

            var transactions = await _unitOfWork.Transactions.FindAsync(
                t => t.TransactionDate >= startDateTime &&
                     t.TransactionDate <= endDateTime &&
                     t.TransactionType == TransactionType.Purchase,
                ct);

            var dailyRevenues = transactions
                .GroupBy(t => DateOnly.FromDateTime(t.TransactionDate))
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(t => t.Amount),
                    TicketsSold = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();

            var totalRevenue = dailyRevenues.Sum(d => d.Revenue);
            var totalTicketsSold = dailyRevenues.Sum(d => d.TicketsSold);

            return new RevenueReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                TotalTicketsSold = totalTicketsSold,
                DailyRevenues = dailyRevenues,
                AverageDailyRevenue = dailyRevenues.Any() ?
                    dailyRevenues.Average(d => d.Revenue) : 0,
                AverageTicketPrice = totalTicketsSold > 0 ?
                    totalRevenue / totalTicketsSold : 0
            };
        }

        public async Task<IEnumerable<PosterSalesDto>> GetTopSellingPostersAsync(
            int topCount = 10,
            CancellationToken ct = default)
        {
            // Получаем все постеры с деталями
            var posters = await _unitOfWork.Posters.GetAllAsync(ct);
            var result = new List<PosterSalesDto>();

            foreach (var poster in posters)
            {
                // Получаем билеты для этого постера
                var posterTickets = await _unitOfWork.TicketRepository
                    .GetTicketsByPosterAsync(poster.Id, ct);

                var soldTickets = posterTickets
                    .Where(t => t.State == TicketState.Sold)
                    .ToList();

                var totalTickets = await _unitOfWork.TicketRepository
                    .GetAvailableTicketsCountAsync(poster.Id, ct) + soldTickets.Count;

                if (totalTickets == 0) continue;

                var revenue = soldTickets
                    .Where(t => t.TicketInfo != null)
                    .Sum(t => t.TicketInfo!.Price);

                var occupancyRate = totalTickets > 0 ?
                    (decimal)soldTickets.Count / totalTickets * 100 : 0;

                result.Add(new PosterSalesDto
                {
                    Poster = _mapper.Map<PosterDto>(poster),
                    TicketsSold = soldTickets.Count,
                    TotalRevenue = revenue,
                    OccupancyRate = occupancyRate
                });
            }

            return result
                .OrderByDescending(p => p.TotalRevenue)
                .ThenByDescending(p => p.TicketsSold)
                .Take(topCount)
                .ToList();
        }

        public async Task<OccupancyReportDto> GetOccupancyReportAsync(
            Guid posterId,
            CancellationToken ct = default)
        {
            var poster = await _unitOfWork.PosterRepository
                .GetPosterWithDetailsAsync(posterId, ct);

            if (poster == null)
                throw new ArgumentException($"Poster with ID {posterId} not found");

            // Получаем все билеты для постера
            var tickets = await _unitOfWork.TicketRepository
                .GetTicketsByPosterAsync(posterId, ct);

            var seatStatuses = tickets.Select(t => new SeatStatusDto
            {
                SeatNumber = t.SeatNumber,
                Status = t.State,
                Type = t.TicketInfo?.TicketType ?? TicketType.Standard,
                Price = t.TicketInfo?.Price ?? 0
            }).ToList();

            var totalSeats = tickets.Count();
            var availableSeats = tickets.Count(t => t.State == TicketState.Available);
            var bookedSeats = tickets.Count(t => t.State == TicketState.Booked);
            var soldSeats = tickets.Count(t => t.State == TicketState.Sold);
            var occupancyRate = totalSeats > 0 ?
                (decimal)(bookedSeats + soldSeats) / totalSeats * 100 : 0;

            // Группируем по типу билета
            var ticketTypeStats = tickets
                .GroupBy(t => t.TicketInfo?.TicketType ?? TicketType.Standard)
                .Select(g => new TicketTypeStatisticsDto
                {
                    TicketType = g.Key,
                    Total = g.Count(),
                    Available = g.Count(t => t.State == TicketState.Available),
                    Booked = g.Count(t => t.State == TicketState.Booked),
                    Sold = g.Count(t => t.State == TicketState.Sold),
                    Revenue = g.Where(t => t.State == TicketState.Sold && t.TicketInfo != null)
                               .Sum(t => t.TicketInfo!.Price)
                })
                .ToList();

            return new OccupancyReportDto
            {
                Poster = _mapper.Map<PosterDto>(poster),
                TotalSeats = totalSeats,
                AvailableSeats = availableSeats,
                BookedSeats = bookedSeats,
                SoldSeats = soldSeats,
                OccupancyRate = occupancyRate,
                Seats = seatStatuses,
                TicketTypeStatistics = ticketTypeStats
            };
        }

        // Дополнительные методы отчетов

        public async Task<CustomerActivityReportDto> GetCustomerActivityReportAsync(
            Guid customerId,
            CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId, ct);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found");

            // Получаем все покупки клиента
            var transactions = await _unitOfWork.Transactions.FindAsync(
                t => t.CustomerId == customerId &&
                     t.TransactionType == TransactionType.Purchase,
                ct);

            // Получаем все бронирования клиента
            var bookings = await _unitOfWork.BookingRepository
                .GetCustomerBookingsAsync(customerId, ct);

            var totalSpent = transactions.Sum(t => t.Amount);
            var ticketsPurchased = transactions.Count();
            var activeBookings = bookings.Count(b => b.Status == BookingStatus.Active);
            var completedBookings = bookings.Count(b => b.Status == BookingStatus.Completed);

            // Любимый жанр
            var purchasedTickets = await _unitOfWork.TicketRepository
                .GetCustomerTicketsAsync(customerId, ct);

            var genrePreferences = purchasedTickets
                .Select(t => t.Poster)
                .Where(p => p != null)
                .SelectMany(p => p!.Genres)
                .GroupBy(g => g)
                .Select(g => new GenrePreferenceDto
                {
                    Genre = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToList();

            return new CustomerActivityReportDto
            {
                Customer = _mapper.Map<CustomerDto>(customer),
                TotalSpent = totalSpent,
                TicketsPurchased = ticketsPurchased,
                ActiveBookings = activeBookings,
                CompletedBookings = completedBookings,
                FirstPurchaseDate = transactions.Any() ?
                    transactions.Min(t => t.TransactionDate) : null,
                LastPurchaseDate = transactions.Any() ?
                    transactions.Max(t => t.TransactionDate) : null,
                GenrePreferences = genrePreferences,
                AverageTicketPrice = ticketsPurchased > 0 ?
                    totalSpent / ticketsPurchased : 0
            };
        }

        public async Task<SalesTrendReportDto> GetSalesTrendReportAsync(
            DateOnly startDate,
            DateOnly endDate,
            string interval = "daily", // daily, weekly, monthly
            CancellationToken ct = default)
        {
            var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
            var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

            var transactions = await _unitOfWork.Transactions.FindAsync(
                t => t.TransactionDate >= startDateTime &&
                     t.TransactionDate <= endDateTime &&
                     t.TransactionType == TransactionType.Purchase,
                ct);

            IEnumerable<SalesTrendDataPointDto> dataPoints;

            switch (interval.ToLower())
            {
                case "weekly":
                    dataPoints = transactions
                        .GroupBy(t => new {
                            Year = t.TransactionDate.Year,
                            Week = System.Globalization.CultureInfo.CurrentCulture.Calendar
                                .GetWeekOfYear(t.TransactionDate,
                                    System.Globalization.CalendarWeekRule.FirstDay,
                                    DayOfWeek.Monday)
                        })
                        .Select(g => new SalesTrendDataPointDto
                        {
                            Period = $"W{g.Key.Week}/{g.Key.Year}",
                            Revenue = g.Sum(t => t.Amount),
                            TicketsSold = g.Count(),
                            AverageTicketPrice = g.Any() ? g.Average(t => t.Amount) : 0
                        })
                        .OrderBy(d => d.Period);
                    break;

                case "monthly":
                    dataPoints = transactions
                        .GroupBy(t => new {
                            Year = t.TransactionDate.Year,
                            Month = t.TransactionDate.Month
                        })
                        .Select(g => new SalesTrendDataPointDto
                        {
                            Period = $"{g.Key.Month:00}/{g.Key.Year}",
                            Revenue = g.Sum(t => t.Amount),
                            TicketsSold = g.Count(),
                            AverageTicketPrice = g.Any() ? g.Average(t => t.Amount) : 0
                        })
                        .OrderBy(d => d.Period);
                    break;

                default: // daily
                    dataPoints = transactions
                        .GroupBy(t => DateOnly.FromDateTime(t.TransactionDate))
                        .Select(g => new SalesTrendDataPointDto
                        {
                            Period = g.Key.ToString("yyyy-MM-dd"),
                            Revenue = g.Sum(t => t.Amount),
                            TicketsSold = g.Count(),
                            AverageTicketPrice = g.Any() ? g.Average(t => t.Amount) : 0
                        })
                        .OrderBy(d => d.Period);
                    break;
            }

            return new SalesTrendReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                Interval = interval,
                DataPoints = dataPoints.ToList(),
                TotalRevenue = dataPoints.Sum(d => d.Revenue),
                TotalTicketsSold = dataPoints.Sum(d => d.TicketsSold),
                PeakRevenue = dataPoints.Any() ? dataPoints.Max(d => d.Revenue) : 0,
                PeakTicketsSold = dataPoints.Any() ? dataPoints.Max(d => d.TicketsSold) : 0
            };
        }
    }
}