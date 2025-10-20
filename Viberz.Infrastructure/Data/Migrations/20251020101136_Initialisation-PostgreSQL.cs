using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Viberz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialisationPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SpotifyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    FavoriteArtists = table.Column<List<string>>(type: "text[]", nullable: true),
                    FavoriteGenres = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XpGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    MinXp = table.Column<int>(type: "integer", nullable: false),
                    GradeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpGrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XpHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EarnedXp = table.Column<int>(type: "integer", nullable: false),
                    TotalXp = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ActivityType = table.Column<int>(type: "integer", nullable: false),
                    Genre = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpHistories", x => x.Id);
                });

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
