using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    state = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
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
                    price = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
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
                    amount = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: false),
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
