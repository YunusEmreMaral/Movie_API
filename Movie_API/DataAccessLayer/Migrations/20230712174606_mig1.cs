using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieGenreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => x.MovieGenreID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserFullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieRelaseYear = table.Column<int>(type: "int", nullable: false),
                    MovieDirector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDuration = table.Column<int>(type: "int", nullable: false),
                    IsWatched = table.Column<bool>(type: "bit", nullable: false),
                    MovieGenreID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieID);
                    table.ForeignKey(
                        name: "FK_Movies_MovieGenres_MovieGenreID",
                        column: x => x.MovieGenreID,
                        principalTable: "MovieGenres",
                        principalColumn: "MovieGenreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Watcheds",
                columns: table => new
                {
                    WatchedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MovieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watcheds", x => x.WatchedID);
                    table.ForeignKey(
                        name: "FK_Watcheds_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Watcheds_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieGenreID",
                table: "Movies",
                column: "MovieGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Watcheds_MovieID",
                table: "Watcheds",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_Watcheds_UserID",
                table: "Watcheds",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Watcheds");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MovieGenres");
        }
    }
}
