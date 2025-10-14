using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viberz.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAddLevelInXpHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "XpHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "XpHistories");
        }
    }
}
