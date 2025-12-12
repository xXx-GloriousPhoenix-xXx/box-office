using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Context
{
    public class BoxOfficeDbContext(DbContextOptions<BoxOfficeDbContext> options) : DbContext(options)
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

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Name);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.Name);
            });

            modelBuilder.Entity<Poster>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Venue)
                    .HasMaxLength(500);

                entity.Property(e => e.Description)
                    .HasMaxLength(2000);

                entity.Property(e => e.ReleaseDate)
                    .IsRequired();

                entity.HasOne(e => e.Author)
                    .WithMany(a => a.Posters)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Genres)
                    .HasConversion(
                        v => string.Join(',', v.Select(g => g.ToString())),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(g => Enum.Parse<PosterGenre>(g))
                              .ToList());

                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.ReleaseDate);
                entity.HasIndex(e => e.AuthorId);
            });

            modelBuilder.Entity<TicketInfo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(e => e.TicketType)
                    .IsRequired();

                entity.HasOne(e => e.Poster)
                    .WithMany(p => p.TicketInfos)
                    .HasForeignKey(e => e.PosterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.PosterId);
                entity.HasIndex(e => e.TicketType);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SeatNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasDefaultValue(TicketState.Available);

                entity.HasOne(e => e.TicketInfo)
                    .WithMany(ti => ti.Tickets)
                    .HasForeignKey(e => e.TicketInfoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Poster)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(e => e.PosterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.PurchasedTickets)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Booking)
                    .WithMany(b => b.Tickets)
                    .HasForeignKey(e => e.BookingId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => new { e.PosterId, e.SeatNumber })
                    .IsUnique();

                entity.HasIndex(e => e.PosterId);
                entity.HasIndex(e => e.TicketInfoId);
                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.BookingId);
                entity.HasIndex(e => e.State);
                entity.HasIndex(e => e.BookedUntil);
                entity.HasIndex(e => e.SoldDate);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.BookingToken)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(e => e.BookingDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ExpiresAt)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(BookingStatus.Active);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Bookings)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.BookingToken)
                    .IsUnique();

                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ExpiresAt);
                entity.HasIndex(e => e.BookingDate);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.TransactionType)
                    .IsRequired();

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasDefaultValue("Card");

                entity.Property(e => e.PaymentReference)
                    .HasMaxLength(200);

                entity.HasOne(e => e.Ticket)
                    .WithMany()
                    .HasForeignKey(e => e.TicketId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.TicketId);
                entity.HasIndex(e => e.TransactionType);
                entity.HasIndex(e => e.TransactionDate);
            });
        }
    }
}
