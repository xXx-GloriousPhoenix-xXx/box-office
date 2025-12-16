using BoxOffice.DAL.Configuration;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
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

            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // ==== 1. ЖАНРЫ (статичные GUID) ====
            var dramaId = new Guid("11111111-1111-1111-1111-111111111111");
            var comedyId = new Guid("22222222-2222-2222-2222-222222222222");
            var tragedyId = new Guid("33333333-3333-3333-3333-333333333333");
            var romanceId = new Guid("44444444-4444-4444-4444-444444444444");
            var historicalId = new Guid("55555555-5555-5555-5555-555555555555");
            var musicalId = new Guid("66666666-6666-6666-6666-666666666666");
            var thrillerId = new Guid("77777777-7777-7777-7777-777777777777");
            var absurdId = new Guid("88888888-8888-8888-8888-888888888888");
            var satireId = new Guid("99999999-9999-9999-9999-999999999999");

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = dramaId, Name = "Drama" },
                new Genre { Id = comedyId, Name = "Comedy" },
                new Genre { Id = tragedyId, Name = "Tragedy" },
                new Genre { Id = romanceId, Name = "Romance" },
                new Genre { Id = historicalId, Name = "Historical" },
                new Genre { Id = musicalId, Name = "Musical" },
                new Genre { Id = thrillerId, Name = "Thriller" },
                new Genre { Id = absurdId, Name = "Absurd" },
                new Genre { Id = satireId, Name = "Satire" }
            );

            // ==== 2. АВТОРЫ ====
            var shakespeareId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var chekhovId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var williamsId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");
            var millerId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var schillerId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
            var moliereId = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff");

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = shakespeareId, Name = "William Shakespeare" },
                new Author { Id = chekhovId, Name = "Anton Chekhov" },
                new Author { Id = williamsId, Name = "Tennessee Williams" },
                new Author { Id = millerId, Name = "Arthur Miller" },
                new Author { Id = schillerId, Name = "Friedrich Schiller" },
                new Author { Id = moliereId, Name = "Molière" }
            );

            // ==== 3. ПОСТЕРЫ (спектакли) ====
            var hamletId = new Guid("aaaaaaaa-1111-1111-1111-111111111111");
            var romeoId = new Guid("aaaaaaaa-2222-2222-2222-222222222222");
            var seagullId = new Guid("bbbbbbbb-1111-1111-1111-111111111111");
            var cherryId = new Guid("bbbbbbbb-2222-2222-2222-222222222222");
            var streetcarId = new Guid("cccccccc-1111-1111-1111-111111111111");
            var salesmanId = new Guid("dddddddd-1111-1111-1111-111111111111");
            var miserId = new Guid("ffffffff-1111-1111-1111-111111111111");

            modelBuilder.Entity<Poster>().HasData(
                new Poster
                {
                    Id = hamletId,
                    Name = "Hamlet",
                    Description = "A tragedy about the Prince of Denmark seeking revenge for his father's murder",
                    Date = new DateOnly(2024, 3, 15),
                    Venue = "Royal Shakespeare Theatre",
                    Duration = 180,
                    AuthorId = shakespeareId
                },
                new Poster
                {
                    Id = romeoId,
                    Name = "Romeo and Juliet",
                    Description = "The classic story of two young star-crossed lovers",
                    Date = new DateOnly(2024, 2, 14),
                    Venue = "Globe Theatre",
                    Duration = 150,
                    AuthorId = shakespeareId
                },
                new Poster
                {
                    Id = seagullId,
                    Name = "The Seagull",
                    Description = "A play about the romantic and artistic conflicts between four characters",
                    Date = new DateOnly(2024, 4, 20),
                    Venue = "Moscow Art Theatre",
                    Duration = 140,
                    AuthorId = chekhovId
                },
                new Poster
                {
                    Id = cherryId,
                    Name = "The Cherry Orchard",
                    Description = "The story of an aristocratic Russian family forced to sell their estate",
                    Date = new DateOnly(2024, 5, 10),
                    Venue = "Bolshoi Theatre",
                    Duration = 160,
                    AuthorId = chekhovId
                },
                new Poster
                {
                    Id = streetcarId,
                    Name = "A Streetcar Named Desire",
                    Description = "The story of Blanche DuBois and her conflicts with her brother-in-law",
                    Date = new DateOnly(2024, 6, 5),
                    Venue = "Broadway Theatre",
                    Duration = 155,
                    AuthorId = williamsId
                },
                new Poster
                {
                    Id = salesmanId,
                    Name = "Death of a Salesman",
                    Description = "The tragedy of Willy Loman, a traveling salesman",
                    Date = new DateOnly(2024, 7, 1),
                    Venue = "National Theatre",
                    Duration = 165,
                    AuthorId = millerId
                },
                new Poster
                {
                    Id = miserId,
                    Name = "The Miser",
                    Description = "A comedy about Harpagon and his obsessive greed",
                    Date = new DateOnly(2024, 8, 15),
                    Venue = "Comédie-Française",
                    Duration = 135,
                    AuthorId = moliereId
                }
            );

            // ==== 4. СВЯЗИ ПОСТЕРОВ С ЖАНРАМИ ====
            modelBuilder.Entity("poster_genres")
                .HasData(
                    // Hamlet - Drama, Tragedy
                    new { poster_id = hamletId, genre_id = dramaId },
                    new { poster_id = hamletId, genre_id = tragedyId },

                    // Romeo and Juliet - Tragedy, Romance
                    new { poster_id = romeoId, genre_id = tragedyId },
                    new { poster_id = romeoId, genre_id = romanceId },

                    // The Seagull - Drama
                    new { poster_id = seagullId, genre_id = dramaId },

                    // The Cherry Orchard - Drama, Satire
                    new { poster_id = cherryId, genre_id = dramaId },
                    new { poster_id = cherryId, genre_id = satireId },

                    // A Streetcar Named Desire - Drama
                    new { poster_id = streetcarId, genre_id = dramaId },

                    // Death of a Salesman - Drama, Tragedy
                    new { poster_id = salesmanId, genre_id = dramaId },
                    new { poster_id = salesmanId, genre_id = tragedyId },

                    // The Miser - Comedy, Satire
                    new { poster_id = miserId, genre_id = comedyId },
                    new { poster_id = miserId, genre_id = satireId }
                );

            // ==== 5. ПОКУПАТЕЛИ ====
            var customer1Id = new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var customer2Id = new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var customer3Id = new Guid("33333333-cccc-cccc-cccc-cccccccccccc");
            var customer4Id = new Guid("44444444-dddd-dddd-dddd-dddddddddddd");
            var customer5Id = new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee");

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = customer1Id, Name = "John Smith", Email = "john.smith@example.com", Phone = "+1-555-0101" },
                new Customer { Id = customer2Id, Name = "Emma Johnson", Email = "emma.johnson@example.com", Phone = "+1-555-0102" },
                new Customer { Id = customer3Id, Name = "Michael Brown", Email = "michael.brown@example.com", Phone = "+1-555-0103" },
                new Customer { Id = customer4Id, Name = "Sarah Davis", Email = "sarah.davis@example.com", Phone = "+1-555-0104" },
                new Customer { Id = customer5Id, Name = "Robert Wilson", Email = "robert.wilson@example.com", Phone = "+1-555-0105" }
            );

            // ==== 6. ИНФОРМАЦИЯ О БИЛЕТАХ ====
            var hamletEconomyId = new Guid("aaaaaaaa-1111-aaaa-aaaa-aaaaaaaaaaaa");
            var hamletVipId = new Guid("aaaaaaaa-2222-bbbb-bbbb-bbbbbbbbbbbb");
            var romeoEconomyId = new Guid("bbbbbbbb-1111-cccc-cccc-cccccccccccc");
            var seagullEconomyId = new Guid("cccccccc-1111-dddd-dddd-dddddddddddd");

            modelBuilder.Entity<TicketInfo>().HasData(
                new TicketInfo
                {
                    Id = hamletEconomyId,
                    PosterId = hamletId,
                    Price = 1500,
                    TotalCount = 2,
                    AvailableCount = 0,
                    SoldCount = 2,
                    BookedCount = 0,
                    TicketType = TicketType.Economy
                },
                new TicketInfo
                {
                    Id = hamletVipId,
                    PosterId = hamletId,
                    Price = 4000,
                    TotalCount = 1,
                    AvailableCount = 0,
                    SoldCount = 0,
                    BookedCount = 1,
                    TicketType = TicketType.VIP
                },
                new TicketInfo
                {
                    Id = romeoEconomyId,
                    PosterId = romeoId,
                    Price = 1200,
                    TotalCount = 1,
                    AvailableCount = 0,
                    SoldCount = 1,
                    BookedCount = 0,
                    TicketType = TicketType.Economy
                },
                new TicketInfo
                {
                    Id = seagullEconomyId,
                    PosterId = seagullId,
                    Price = 1000,
                    TotalCount = 1,
                    AvailableCount = 0,
                    SoldCount = 0,
                    BookedCount = 1,
                    TicketType = TicketType.Economy
                }
            );

            // ==== 7. БИЛЕТЫ ====
            var ticket1Id = new Guid("11111111-1111-1111-aaaa-aaaaaaaaaaaa");
            var ticket2Id = new Guid("22222222-2222-2222-bbbb-bbbbbbbbbbbb");
            var ticket3Id = new Guid("33333333-3333-3333-cccc-cccccccccccc");
            var ticket4Id = new Guid("44444444-4444-4444-dddd-dddddddddddd");
            var ticket5Id = new Guid("55555555-5555-5555-eeee-eeeeeeeeeeee");

            // ==== 8. БРОНИРОВАНИЯ ====
            var booking1Id = new Guid("aaaaaaaa-1111-bbbb-cccc-dddddddddddd");
            var booking2Id = new Guid("bbbbbbbb-2222-cccc-dddd-eeeeeeeeeeee");

            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = ticket1Id,
                    TicketInfoId = hamletEconomyId,
                    SeatNumber = "A101",
                    TicketState = TicketState.Sold,
                    SoldDate = new DateOnly(2024, 2, 10),
                    CustomerId = customer1Id
                },
                new Ticket
                {
                    Id = ticket2Id,
                    TicketInfoId = hamletVipId,
                    SeatNumber = "V001",
                    TicketState = TicketState.Booked,
                    SoldDate = null,
                    CustomerId = customer2Id,
                    BookingId = booking1Id
                },
                new Ticket
                {
                    Id = ticket3Id,
                    TicketInfoId = hamletEconomyId,
                    SeatNumber = "A102",
                    TicketState = TicketState.Sold,
                    SoldDate = new DateOnly(2024, 2, 11),
                    CustomerId = customer3Id
                },
                new Ticket
                {
                    Id = ticket4Id,
                    TicketInfoId = romeoEconomyId,
                    SeatNumber = "B201",
                    TicketState = TicketState.Sold,
                    SoldDate = new DateOnly(2024, 1, 25),
                    CustomerId = customer4Id
                },
                new Ticket
                {
                    Id = ticket5Id,
                    TicketInfoId = seagullEconomyId,
                    SeatNumber = "C301",
                    TicketState = TicketState.Booked,
                    SoldDate = null,
                    CustomerId = customer5Id,
                    BookingId = booking2Id
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = booking1Id,
                    BookedAt = new DateOnly(2024, 2, 9),
                    ExpiresAt = new DateOnly(2024, 2, 12),
                    BookingToken = "BOOK123456",
                    State = BookingState.Active,
                    TicketId = ticket2Id
                },
                new Booking
                {
                    Id = booking2Id,
                    BookedAt = new DateOnly(2024, 1, 28),
                    ExpiresAt = new DateOnly(2024, 1, 31),
                    BookingToken = "BOOK654321",
                    State = BookingState.Expired,
                    TicketId = ticket5Id
                }
            );

            // ==== 9. ТРАНЗАКЦИИ ====
            var transaction1Id = new Guid("11111111-aaaa-bbbb-cccc-dddddddddddd");
            var transaction2Id = new Guid("22222222-bbbb-cccc-dddd-eeeeeeeeeeee");
            var transaction3Id = new Guid("33333333-cccc-dddd-eeee-ffffffffffff");

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = transaction1Id,
                    Amount = 1500,
                    Date = new DateOnly(2024, 2, 10),
                    PaymentMethod = PaymentMethod.CardOnline,
                    TransactionType = TransactionType.Purchase,
                    CustomerId = customer1Id,
                    TicketId = ticket1Id
                },
                new Transaction
                {
                    Id = transaction2Id,
                    Amount = 1500,
                    Date = new DateOnly(2024, 2, 11),
                    PaymentMethod = PaymentMethod.CashAtVenue,
                    TransactionType = TransactionType.Purchase,
                    CustomerId = customer3Id,
                    TicketId = ticket3Id
                },
                new Transaction
                {
                    Id = transaction3Id,
                    Amount = 1200,
                    Date = new DateOnly(2024, 1, 25),
                    PaymentMethod = PaymentMethod.CardAtVenue,
                    TransactionType = TransactionType.Purchase,
                    CustomerId = customer4Id,
                    TicketId = ticket4Id
                }
            );
        }
    }
}
