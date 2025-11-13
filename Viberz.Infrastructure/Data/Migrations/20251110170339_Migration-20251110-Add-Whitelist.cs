using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Viberz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration20251110AddWhitelist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Whitelist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whitelist", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "SpotifyId" },
                values: new object[,]
                {
                    { 10, "Progressive House", "3qSskUmTASIyuIKWTfZT2J" },
                    { 11, "Mid Tempo", "3hUcNAkx9fQ20kREAR22A7" },
                    { 12, "Future House", "6irfmXy7b12btvyYPrpEA4" },
                    { 13, "Hardstyle", "2exynTofOAgLHuonZaImx2" },
                    { 14, "Big Room", "4bApSsicsTPhjR3zt2UPmg" }
                });

            migrationBuilder.InsertData(
                table: "Whitelist",
                columns: new[] { "Id", "Email" },
                values: new object[,]
                {
                    { 1, "thomas.bfrd@gmail.com" },
                    { 2, "micanille@hotmail.fr" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Whitelist");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
