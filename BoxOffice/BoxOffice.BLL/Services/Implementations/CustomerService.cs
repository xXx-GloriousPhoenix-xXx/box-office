using AutoMapper;
using BoxOffice.BLL;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CustomerService> logger)
        : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CustomerService> _logger = logger;

        [LoggerMessage(LogLevel.Information, "Customer created: {Name} (ID: {Id})")]
        partial void LogCustomerCreated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Customer updated: {Name} (ID: {Id})")]
        partial void LogCustomerUpdated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Customer deleted: {Name} (ID: {Id})")]
        partial void LogCustomerDeleted(string name, Guid id);

        public async Task<GetCustomerDto> AddAsync(CreateCustomerDto createDto, CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.Customers
                .ExistsAsync(c => c.Email.ToLower() == createDto.Email.ToLower(), ct);

            if (emailExists)
            {
                throw new ValidationException($"Customer with email '{createDto.Email}' already exists");
            }

            var phoneExists = await _unitOfWork.Customers
                .ExistsAsync(c => c.Phone == createDto.PhoneNumber, ct);

            if (phoneExists)
            {
                throw new ValidationException($"Customer with phone number '{createDto.PhoneNumber}' already exists");
            }

            var customer = _mapper.Map<Customer>(createDto);

            _unitOfWork.Customers.Add(customer);
            await _unitOfWork.CompleteAsync(ct);

            LogCustomerCreated(customer.Name, customer.Id);

            return _mapper.Map<GetCustomerDto>(customer);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers
                .GetByIdAsync(id, ct,
                    c => c.Transactions,
                    c => c.Tickets);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }

            if (customer.Transactions.Count > 0 || customer.Tickets.Count > 0)
            {
                throw new BusinessException(
                    $"Cannot delete customer with {customer.Transactions.Count} transactions " +
                    $"and {customer.Tickets.Count} tickets. " +
                    "Delete related transactions and tickets first.");
            }

            _unitOfWork.Customers.Delete(customer);
            await _unitOfWork.CompleteAsync(ct);

            LogCustomerDeleted(customer.Name, customer.Id);
        }

        public async Task<PagedResponse<GetCustomerDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (itemsPerPage < 1)
            {
                itemsPerPage = 10;
            }
            if (itemsPerPage > 100)
            {
                itemsPerPage = 100;
            }

            var query = _unitOfWork.Customers.AsQueryable();
            var totalCount = await query.CountAsync(ct);

            var customers = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var customerDtos = _mapper.Map<List<GetCustomerDto>>(customers);

            return new PagedResponse<GetCustomerDto>
            {
                Items = customerDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetCustomerWithBookingsDto> GetCustomerBookingsAsync(Guid id, CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers
                .GetByIdAsync(id, ct,
                    includes: c => c.Tickets);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }

            var result = new GetCustomerWithBookingsDto();

            foreach (var ticket in customer.Tickets)
            {
                var ticketWithBooking = await _unitOfWork.Tickets
                    .GetByIdAsync(ticket.Id, ct,
                        includes: t => t.Booking);

                if (ticketWithBooking?.Booking != null)
                {
                    var bookingDto = _mapper.Map<GetBookingAdditionDto>(ticketWithBooking.Booking);
                    result.Bookings.Add(bookingDto);
                }
            }

            return result;
        }

        public async Task<GetCustomerWithTicketsDto> GetCustomerTicketsAsync(Guid id, CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers
                .GetByIdAsync(id, ct,
                    includes: c => c.Tickets);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }

            var result = _mapper.Map<GetCustomerWithTicketsDto>(customer);
            result.Tickets = _mapper.Map<List<GetTicketAdditionDto>>(customer.Tickets);

            // Load additional ticket details if needed
            //foreach (var ticket in customer.Tickets)
            //{
            //    var fullTicket = await _unitOfWork.Tickets
            //        .GetByIdAsync(ticket.Id, ct,
            //            includes: t => t.TicketInfo);

            //    if (fullTicket?.TicketInfo != null)
            //    {
            //        // You can add more details here if needed
            //    }
            //}

            return result;
        }

        public async Task<GetCustomerDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, ct);
            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }

            return _mapper.Map<GetCustomerDto>(customer);
        }

        public async Task<GetCustomerDto> UpdateAsync(Guid id, UpdateCustomerDto updateDto, CancellationToken ct = default)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, ct);
            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email)
                && updateDto.Email != customer.Email)
            {
                var emailExists = await _unitOfWork.Customers
                    .ExistsAsync(c => c.Email.ToLower() == updateDto.Email.ToLower().Trim()
                           && c.Id != id, ct);

                if (emailExists)
                {
                    throw new ValidationException($"Customer with email '{updateDto.Email}' already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber)
                && updateDto.PhoneNumber != customer.Phone)
            {
                var phoneExists = await _unitOfWork.Customers
                    .ExistsAsync(c => c.Phone == updateDto.PhoneNumber.Trim()
                           && c.Id != id, ct);

                if (phoneExists)
                {
                    throw new ValidationException($"Customer with phone number '{updateDto.PhoneNumber}' already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                customer.Name = updateDto.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                customer.Email = updateDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            {
                customer.Phone = updateDto.PhoneNumber;
            }

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync(ct);

            LogCustomerUpdated(customer.Name, customer.Id);

            return _mapper.Map<GetCustomerDto>(customer);
        }
    }
}
