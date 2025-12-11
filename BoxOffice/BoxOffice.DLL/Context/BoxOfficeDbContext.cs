using BoxOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Context
{
    public class BoxOfficeDbContext : DbContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Poster> Posters { get; set; }
        DbSet<Ticket> Tickets { get; set; }
        DbSet<TicketInfo> TicketInfos { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
