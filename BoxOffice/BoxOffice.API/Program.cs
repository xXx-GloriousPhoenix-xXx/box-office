//using BoxOffice.BLL.Interfaces;
//using BoxOffice.BLL.Mapping;
//using BoxOffice.BLL.Services;
using BoxOffice.DAL.Context;
using BoxOffice.DAL.Interfaces;
using BoxOffice.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoxOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddScoped<IPosterService, PosterService>();
//builder.Services.AddScoped<ITicketService, TicketService>();
//builder.Services.AddScoped<IBookingService, BookingService>();
//builder.Services.AddScoped<IReportService, ReportService>();
//builder.Services.AddScoped<IAuthorService, AuthorService>();

//builder.Services.AddAutoMapper(typeof(PosterProfile).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(
            JsonNamingPolicy.CamelCase,
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