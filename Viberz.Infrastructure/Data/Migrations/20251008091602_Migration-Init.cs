using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Viberz.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpotifyId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FavoriteArtists = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FavoriteGenres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "XpGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MinXp = table.Column<int>(type: "int", nullable: false),
                    GradeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpGrades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "XpHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EarnedXp = table.Column<int>(type: "int", nullable: false),
                    TotalXp = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpHistories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "SpotifyId" },
                values: new object[,]
                {
                    { 1, "Bass House", "0Y2Dt0Vfg3VSYBKd2BRGyx" },
                    { 2, "EDM Trap", "4QXVqNH5XVCDAlAS1HocVW" },
                    { 3, "Dubstep", "6qxaYSiO7LVEJE6dsjC9QU" },
                    { 4, "Drum & Bass", "0UkGPS5GMhO0QmJx9MvbJm" },
                    { 5, "Tech House", "2hDT4wx3d3hFNwKMgIKNiB" },
                    { 6, "Hard Techno", "6LsBdAO09tOMgkt8RJ5ktv" },
                    { 7, "Garage UK", "1HzqMHV1TMMZjD0POdlww0" },
                    { 8, "Hyper Techno", "5WhatiRekXXBNRXeO6jXmV" },
                    { 9, "Stutter House", "08oVyHSMPq79FNRvdxnnPK" }
                });

            migrationBuilder.InsertData(
                table: "XpGrades",
                columns: new[] { "Id", "GradeName", "Level", "MinXp" },
                values: new object[,]
                {
                    { 1, "Newbie", 1, 0 },
                    { 2, "Beat Beginner", 2, 50 },
                    { 3, "Bass Novice", 3, 120 },
                    { 4, "Stage Starter", 4, 250 },
                    { 5, "Drop Apprentice", 5, 450 },
                    { 6, "Groove Tinkerer", 6, 800 },
                    { 7, "Crowd Controller", 7, 1300 },
                    { 8, "Drop Disciple", 8, 2100 },
                    { 9, "Festival Vanguard", 9, 3200 },
                    { 10, "Bass Boss", 10, 4700 },
                    { 11, "Sound Sorcerer", 11, 6700 },
                    { 12, "Legendary Master", 12, 9500 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "XpGrades");

            migrationBuilder.DropTable(
                name: "XpHistories");
        }
    }
}
