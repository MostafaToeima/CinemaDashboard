using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaDashboard.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MainImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieActors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MovieActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieActors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieSubImages",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSubImages", x => new { x.MovieId, x.Img });
                    table.ForeignKey(
                        name: "FK_MovieSubImages_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Bio", "Img", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Famous Egyptian action actor", "default.png", "Ahmed Ezz", true },
                    { 2, "Renowned Egyptian drama actress", "default.png", "Mona Zaki", true },
                    { 3, "Versatile Egyptian movie star", "default.png", "Karim Abdel Aziz", true },
                    { 4, "Award-winning actress", "default.png", "Hend Sabry", true },
                    { 5, "Top Egyptian comedy actor", "default.png", "Ahmed Helmy", true },
                    { 6, "Acclaimed drama actress", "default.png", "Nelly Karim", false }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "High energy action movies", "Action", true },
                    { 2, "Emotional and story-driven", "Drama", true },
                    { 3, "Light-hearted and funny", "Comedy", true },
                    { 4, "Scary and thrilling movies", "Horror", false },
                    { 5, "Science fiction and futurism", "Sci-Fi", true }
                });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Description", "Img", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Biggest cinema in Cairo", "default.png", "Grand Cinema Cairo", true },
                    { 2, "Located in Alexandria", "default.png", "Stars Cinema Alex", true },
                    { 3, "Modern screens in Giza", "default.png", "Nova Screens Giza", true },
                    { 4, "Family-friendly cinema", "default.png", "CinePlex Maadi", false },
                    { 5, "Premium experience", "default.png", "Royal Cinema Heliopolis", true }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "CinemaId", "DateTime", "Description", "MainImg", "Name", "Price", "Status" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Epic action in the desert", "default.png", "Desert Storm", 120m, true },
                    { 2, 2, 2, new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A love story gone wrong", "default.png", "Broken Hearts", 95m, true },
                    { 3, 3, 3, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Non-stop comedy", "default.png", "Laugh Factory", 80m, true },
                    { 4, 4, 4, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Horror thriller", "default.png", "Dark Shadows", 100m, false },
                    { 5, 5, 5, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sci-fi adventure", "default.png", "Galaxy Quest", 150m, true },
                    { 6, 1, 1, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Action in the streets", "default.png", "City of Fire", 110m, true },
                    { 7, 2, 2, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emotional family drama", "default.png", "Silent Tears", 90m, true },
                    { 8, 3, 3, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family comedy for all ages", "default.png", "Just Kidding", 75m, true },
                    { 9, 4, 4, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supernatural horror", "default.png", "The Awakening", 105m, false },
                    { 10, 5, 5, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Space exploration epic", "default.png", "Beyond the Stars", 160m, true }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookedAt", "CustomerName", "CustomerPhone", "MovieId", "Seats" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed Ali", "01012345678", 1, 2 },
                    { 2, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara Mohamed", "01198765432", 2, 1 },
                    { 3, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khaled Hassan", "01234567890", 3, 3 },
                    { 4, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed Ali", "01012345678", 5, 2 },
                    { 5, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nour Tarek", "01556677889", 6, 4 }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 4, 2 },
                    { 5, 3 },
                    { 3, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 2, 7 },
                    { 5, 8 },
                    { 6, 9 },
                    { 3, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MovieId",
                table: "Bookings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_ActorId",
                table: "MovieActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CinemaId",
                table: "Movies",
                column: "CinemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "MovieActors");

            migrationBuilder.DropTable(
                name: "MovieSubImages");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Cinemas");
        }
    }
}
