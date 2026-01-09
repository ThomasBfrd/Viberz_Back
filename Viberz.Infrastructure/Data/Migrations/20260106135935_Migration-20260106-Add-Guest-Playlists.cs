using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Viberz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration20260106AddGuestPlaylists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGuestGenre",
                table: "Genres",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsGuestGenre",
                value: false);

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsGuestGenre", "Name", "SpotifyId" },
                values: new object[,]
                {
                    { 101, true, "Bass House", "0toSf2qoXdSYTsICqzGUbr" },
                    { 102, true, "EDM Trap", "6RZZqNGqXjAIs9KiOTmOEH" },
                    { 103, true, "Dubstep", "3M3vT1UGM1PjPAbwqIJpxB" },
                    { 104, true, "Drum & Bass", "5GbXD4WM4m85yfn89MhGlV" },
                    { 105, true, "Future House", "1HVD4YYRmfUL6JEyEXmedp" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DropColumn(
                name: "IsGuestGenre",
                table: "Genres");
        }
    }
}
