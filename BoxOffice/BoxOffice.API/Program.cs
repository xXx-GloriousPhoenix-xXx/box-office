using BoxOffice.BLL.Mapping;
using BoxOffice.BLL.Services.Implementations;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Context;
using BoxOffice.DAL.Repositories.Implementations;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoxOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AuthorProfile).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<IAuthorService, AuthorService>()
    .AddScoped<IGenreService, GenreService>()
    .AddScoped<IPosterService, PosterService>()
    .AddScoped<ITicketInfoService, TicketInfoService>()
    .AddScoped<ITicketService, TicketService>()
    .AddScoped<IBookingService, BookingService>()
    .AddScoped<ITransactionService, TransactionService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(
            JsonNamingPolicy.KebabCaseLower,
            allowIntegerValues: false));
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BoxOfficeDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();