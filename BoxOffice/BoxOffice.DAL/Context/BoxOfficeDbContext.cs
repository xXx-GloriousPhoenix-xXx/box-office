using BoxOffice.DAL.Configuration;
using BoxOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Context
{
    public class BoxOfficeDbContext(DbContextOptions<BoxOfficeDbContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Poster> Posters { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketInfo> TicketInfos { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .ApplyConfiguration(new BookingConfiguration())
                .ApplyConfiguration(new PosterConfiguration())
                .ApplyConfiguration(new TicketConfiguration())
                .ApplyConfiguration(new TicketInfoConfiguration())
                .ApplyConfiguration(new TransactionConfiguration());
        }
    }
}
