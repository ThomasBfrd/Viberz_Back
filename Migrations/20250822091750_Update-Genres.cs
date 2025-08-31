using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viberz.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_ArtistSearchDTO_ArtistSearchDTOId",
                table: "Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Users_UserId",
                table: "Genres");

            migrationBuilder.DropTable(
                name: "SpotifyImage");

            migrationBuilder.DropTable(
                name: "ArtistSearchDTO");

            migrationBuilder.DropIndex(
                name: "IX_Genres_ArtistSearchDTOId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_UserId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ArtistSearchDTOId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteArtists",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteGenres",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteArtists",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FavoriteGenres",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ArtistSearchDTOId",
                table: "Genres",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Genres",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArtistSearchDTO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistSearchDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistSearchDTO_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpotifyImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArtistSearchDTOId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Width = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpotifyImage_ArtistSearchDTO_ArtistSearchDTOId",
                        column: x => x.ArtistSearchDTOId,
                        principalTable: "ArtistSearchDTO",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ArtistSearchDTOId", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ArtistSearchDTOId",
                table: "Genres",
                column: "ArtistSearchDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_UserId",
                table: "Genres",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSearchDTO_UserId",
                table: "ArtistSearchDTO",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyImage_ArtistSearchDTOId",
                table: "SpotifyImage",
                column: "ArtistSearchDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_ArtistSearchDTO_ArtistSearchDTOId",
                table: "Genres",
                column: "ArtistSearchDTOId",
                principalTable: "ArtistSearchDTO",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Users_UserId",
                table: "Genres",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
