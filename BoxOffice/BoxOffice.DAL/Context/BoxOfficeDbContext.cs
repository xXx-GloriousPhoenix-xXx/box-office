using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
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

            // ===== Author — Poster (1:N) =====
            modelBuilder.Entity<Poster>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Posters)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            // Не можна видалити автора, якщо у нього є афіши

            // ===== Poster — Genre (M:N) =====
            modelBuilder.Entity<Poster>()
                .HasMany(p => p.Genres)
                .WithMany(g => g.Posters)
                .UsingEntity<Dictionary<string, object>>(
                    "poster_genres",
                    j => j
                        .HasOne<Genre>()
                        .WithMany()
                        .HasForeignKey("genre_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Poster>()
                        .WithMany()
                        .HasForeignKey("poster_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("poster_id", "genre_id");
                    });
            // Якщо видаляється афіша - видаляються зв'язки з жанром
            // Якщо видаляється жанр - видаляються зв'язки з афішами

            modelBuilder.Entity<TicketInfo>(e =>
            {
                // ===== Poster — TicketInfo (1:N) =====
                e.HasOne(ti => ti.Poster)
                .WithMany(p => p.TicketInfos)
                .HasForeignKey(ti => ti.PosterId)
                .OnDelete(DeleteBehavior.Cascade);
                // Якщо видаляється афіша - видаляються зв'язані з нею види квитків

                e.Property(e => e.Price)
                    .HasPrecision(7, 2)
                    .IsRequired();

                e.Property(e => e.TicketType)
                    .HasConversion<string>()
                    .HasMaxLength(16);
            });



            // ===== TicketInfo — Ticket (1:N) =====
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.TicketInfo)
                .WithMany(ti => ti.Tickets)
                .HasForeignKey(t => t.TicketInfoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Не можна видалити тип квитку, якщо у нього є афіша

            // ===== Customer — Ticket (1:N, optional) =====
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
            // Якщо видалити клієнта - CustomerId записується як null

            // ===== Booking — Ticket (1:1) =====
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Ticket)
                .WithOne(t => t.Booking)
                .HasForeignKey<Ticket>(t => t.BookingId)
                .OnDelete(DeleteBehavior.SetNull);
            // Якщо бронювання - BookingId записується як null

            // ===== Ticket — Transaction (1:N) =====
            modelBuilder.Entity<Transaction>()
                .HasOne(tr => tr.Ticket)
                .WithMany(t => t.Transactions)
                .HasForeignKey(tr => tr.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
            // Якщо видалити білет - транзації видаляються

            // ===== Customer — Transaction (1:N) =====
            modelBuilder.Entity<Transaction>()
                .HasOne(tr => tr.Customer)
                .WithMany(c => c.Transactions)
                .HasForeignKey(tr => tr.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            // Якщо видалити клієнта - транзації видаляються
        }
    }
}
