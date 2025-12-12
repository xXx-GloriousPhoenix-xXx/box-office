using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxOffice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    author_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    customer_email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    customer_phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "posters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    poster_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    poster_description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    poster_genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    release_date = table.Column<DateOnly>(type: "date", nullable: false),
                    performance_venue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    performance_duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posters", x => x.id);
                    table.ForeignKey(
                        name: "FK_posters_authors_author_id",
                        column: x => x.author_id,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    booking_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    booking_token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tickets_info",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    poster_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticket_type = table.Column<int>(type: "int", nullable: false),
                    ticket_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_tickets = table.Column<int>(type: "int", nullable: false),
                    available_tickets = table.Column<int>(type: "int", nullable: false),
                    sold_tickets = table.Column<int>(type: "int", nullable: false),
                    booked_tickets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_info_posters_poster_id",
                        column: x => x.poster_id,
                        principalTable: "posters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ticket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticket_info_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    poster_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seat_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ticket_state = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    booking_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    booked_until = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sold_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    unique_booking_token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ticket_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_posters_poster_id",
                        column: x => x.poster_id,
                        principalTable: "posters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_tickets_info_ticket_info_id",
                        column: x => x.ticket_info_id,
                        principalTable: "tickets_info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    transaction_type = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Card"),
                    payment_reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_ticket_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "ticket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_authors_author_name",
                table: "authors",
                column: "author_name");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_date",
                table: "bookings",
                column: "booking_date");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_token",
                table: "bookings",
                column: "booking_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_customer_id",
                table: "bookings",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_expires_at",
                table: "bookings",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_status",
                table: "bookings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_customers_customer_email",
                table: "customers",
                column: "customer_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customers_customer_name",
                table: "customers",
                column: "customer_name");

            migrationBuilder.CreateIndex(
                name: "IX_posters_author_id",
                table: "posters",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_posters_poster_name",
                table: "posters",
                column: "poster_name");

            migrationBuilder.CreateIndex(
                name: "IX_posters_release_date",
                table: "posters",
                column: "release_date");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_booked_until",
                table: "ticket",
                column: "booked_until");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_booking_id",
                table: "ticket",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_customer_id",
                table: "ticket",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_poster_id",
                table: "ticket",
                column: "poster_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_poster_id_seat_number",
                table: "ticket",
                columns: new[] { "poster_id", "seat_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ticket_sold_date",
                table: "ticket",
                column: "sold_date");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_ticket_info_id",
                table: "ticket",
                column: "ticket_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_ticket_state",
                table: "ticket",
                column: "ticket_state");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_info_poster_id",
                table: "tickets_info",
                column: "poster_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_info_ticket_type",
                table: "tickets_info",
                column: "ticket_type");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_customer_id",
                table: "transactions",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_ticket_id",
                table: "transactions",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_transaction_date",
                table: "transactions",
                column: "transaction_date");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_transaction_type",
                table: "transactions",
                column: "transaction_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "ticket");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "tickets_info");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "posters");

            migrationBuilder.DropTable(
                name: "authors");
        }
    }
}
