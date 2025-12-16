using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BoxOffice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    booked_at = table.Column<DateOnly>(type: "date", nullable: false),
                    expires_at = table.Column<DateOnly>(type: "date", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ticket_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "posters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    performance_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    performance_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    release_date = table.Column<DateOnly>(type: "date", nullable: false),
                    performance_venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    performance_duration = table.Column<int>(type: "int", nullable: false),
                    performance_author = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posters", x => x.id);
                    table.ForeignKey(
                        name: "FK_posters_authors_performance_author",
                        column: x => x.performance_author,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "poster_genres",
                columns: table => new
                {
                    poster_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poster_genres", x => new { x.poster_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_poster_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_poster_genres_posters_poster_id",
                        column: x => x.poster_id,
                        principalTable: "posters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    poster = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    total_tickets = table.Column<int>(type: "int", nullable: false),
                    available_tickets = table.Column<int>(type: "int", nullable: false),
                    sold_tickets = table.Column<int>(type: "int", nullable: false),
                    booked_tickets = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_infos", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_infos_posters_poster",
                        column: x => x.poster,
                        principalTable: "posters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticket_info = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seat_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    state = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    booking = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    sold_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_bookings_booking",
                        column: x => x.booking,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tickets_customers_customer",
                        column: x => x.customer,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tickets_ticket_infos_ticket_info",
                        column: x => x.ticket_info,
                        principalTable: "ticket_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    type = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    customer = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticket = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_customers_customer",
                        column: x => x.customer,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_tickets_ticket",
                        column: x => x.ticket,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "authors",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "William Shakespeare" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Anton Chekhov" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Tennessee Williams" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Arthur Miller" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Friedrich Schiller" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Molière" }
                });

            migrationBuilder.InsertData(
                table: "bookings",
                columns: new[] { "id", "booked_at", "token", "expires_at", "state", "ticket_id" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-bbbb-cccc-dddddddddddd"), new DateOnly(2024, 2, 9), "BOOK123456", new DateOnly(2024, 2, 12), "Active", new Guid("22222222-2222-2222-bbbb-bbbbbbbbbbbb") },
                    { new Guid("bbbbbbbb-2222-cccc-dddd-eeeeeeeeeeee"), new DateOnly(2024, 1, 28), "BOOK654321", new DateOnly(2024, 1, 31), "Expired", new Guid("55555555-5555-5555-eeee-eeeeeeeeeeee") }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "email", "name", "phone_number" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "john.smith@example.com", "John Smith", "+1-555-0101" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "emma.johnson@example.com", "Emma Johnson", "+1-555-0102" },
                    { new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), "michael.brown@example.com", "Michael Brown", "+1-555-0103" },
                    { new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), "sarah.davis@example.com", "Sarah Davis", "+1-555-0104" },
                    { new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), "robert.wilson@example.com", "Robert Wilson", "+1-555-0105" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Drama" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Comedy" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Tragedy" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Romance" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Historical" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Musical" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Thriller" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Absurd" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Satire" }
                });

            migrationBuilder.InsertData(
                table: "posters",
                columns: new[] { "id", "performance_author", "release_date", "performance_description", "performance_duration", "performance_name", "performance_venue" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateOnly(2024, 3, 15), "A tragedy about the Prince of Denmark seeking revenge for his father's murder", 180, "Hamlet", "Royal Shakespeare Theatre" },
                    { new Guid("aaaaaaaa-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateOnly(2024, 2, 14), "The classic story of two young star-crossed lovers", 150, "Romeo and Juliet", "Globe Theatre" },
                    { new Guid("bbbbbbbb-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateOnly(2024, 4, 20), "A play about the romantic and artistic conflicts between four characters", 140, "The Seagull", "Moscow Art Theatre" },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateOnly(2024, 5, 10), "The story of an aristocratic Russian family forced to sell their estate", 160, "The Cherry Orchard", "Bolshoi Theatre" },
                    { new Guid("cccccccc-1111-1111-1111-111111111111"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateOnly(2024, 6, 5), "The story of Blanche DuBois and her conflicts with her brother-in-law", 155, "A Streetcar Named Desire", "Broadway Theatre" },
                    { new Guid("dddddddd-1111-1111-1111-111111111111"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateOnly(2024, 7, 1), "The tragedy of Willy Loman, a traveling salesman", 165, "Death of a Salesman", "National Theatre" },
                    { new Guid("ffffffff-1111-1111-1111-111111111111"), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateOnly(2024, 8, 15), "A comedy about Harpagon and his obsessive greed", 135, "The Miser", "Comédie-Française" }
                });

            migrationBuilder.InsertData(
                table: "poster_genres",
                columns: new[] { "genre_id", "poster_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-2222-2222-2222-222222222222") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("aaaaaaaa-2222-2222-2222-222222222222") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-2222-2222-2222-222222222222") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("bbbbbbbb-2222-2222-2222-222222222222") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cccccccc-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("dddddddd-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("dddddddd-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("ffffffff-1111-1111-1111-111111111111") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("ffffffff-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "ticket_infos",
                columns: new[] { "id", "available_tickets", "booked_tickets", "poster", "price", "sold_tickets", "type", "total_tickets" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-aaaa-aaaa-aaaaaaaaaaaa"), 0, 0, new Guid("aaaaaaaa-1111-1111-1111-111111111111"), 1500m, 2, "Economy", 2 },
                    { new Guid("aaaaaaaa-2222-bbbb-bbbb-bbbbbbbbbbbb"), 0, 1, new Guid("aaaaaaaa-1111-1111-1111-111111111111"), 4000m, 0, "VIP", 1 },
                    { new Guid("bbbbbbbb-1111-cccc-cccc-cccccccccccc"), 0, 0, new Guid("aaaaaaaa-2222-2222-2222-222222222222"), 1200m, 1, "Economy", 1 },
                    { new Guid("cccccccc-1111-dddd-dddd-dddddddddddd"), 0, 1, new Guid("bbbbbbbb-1111-1111-1111-111111111111"), 1000m, 0, "Economy", 1 }
                });

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "id", "booking", "customer", "seat_number", "sold_date", "ticket_info", "state" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-aaaa-aaaaaaaaaaaa"), null, new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "A101", new DateOnly(2024, 2, 10), new Guid("aaaaaaaa-1111-aaaa-aaaa-aaaaaaaaaaaa"), "Sold" },
                    { new Guid("22222222-2222-2222-bbbb-bbbbbbbbbbbb"), new Guid("aaaaaaaa-1111-bbbb-cccc-dddddddddddd"), new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "V001", null, new Guid("aaaaaaaa-2222-bbbb-bbbb-bbbbbbbbbbbb"), "Booked" },
                    { new Guid("33333333-3333-3333-cccc-cccccccccccc"), null, new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), "A102", new DateOnly(2024, 2, 11), new Guid("aaaaaaaa-1111-aaaa-aaaa-aaaaaaaaaaaa"), "Sold" },
                    { new Guid("44444444-4444-4444-dddd-dddddddddddd"), null, new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), "B201", new DateOnly(2024, 1, 25), new Guid("bbbbbbbb-1111-cccc-cccc-cccccccccccc"), "Sold" },
                    { new Guid("55555555-5555-5555-eeee-eeeeeeeeeeee"), new Guid("bbbbbbbb-2222-cccc-dddd-eeeeeeeeeeee"), new Guid("55555555-eeee-eeee-eeee-eeeeeeeeeeee"), "C301", null, new Guid("cccccccc-1111-dddd-dddd-dddddddddddd"), "Booked" }
                });

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "id", "amount", "customer", "date", "payment_method", "ticket", "type" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-dddddddddddd"), 1500m, new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateOnly(2024, 2, 10), "CardOnline", new Guid("11111111-1111-1111-aaaa-aaaaaaaaaaaa"), "Purchase" },
                    { new Guid("22222222-bbbb-cccc-dddd-eeeeeeeeeeee"), 1500m, new Guid("33333333-cccc-cccc-cccc-cccccccccccc"), new DateOnly(2024, 2, 11), "CashAtVenue", new Guid("33333333-3333-3333-cccc-cccccccccccc"), "Purchase" },
                    { new Guid("33333333-cccc-dddd-eeee-ffffffffffff"), 1200m, new Guid("44444444-dddd-dddd-dddd-dddddddddddd"), new DateOnly(2024, 1, 25), "CardAtVenue", new Guid("44444444-4444-4444-dddd-dddddddddddd"), "Purchase" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_poster_genres_genre_id",
                table: "poster_genres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_posters_performance_author",
                table: "posters",
                column: "performance_author");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_infos_poster",
                table: "ticket_infos",
                column: "poster");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_booking",
                table: "tickets",
                column: "booking",
                unique: true,
                filter: "[booking] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_customer",
                table: "tickets",
                column: "customer");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ticket_info",
                table: "tickets",
                column: "ticket_info");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_customer",
                table: "transactions",
                column: "customer");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_ticket",
                table: "transactions",
                column: "ticket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "poster_genres");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "ticket_infos");

            migrationBuilder.DropTable(
                name: "posters");

            migrationBuilder.DropTable(
                name: "authors");
        }
    }
}
