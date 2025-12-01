using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viberz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration20251123TempoUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Whitelist",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Whitelist",
                columns: new[] { "Id", "Email" },
                values: new object[] { 2, "micanille@hotmail.fr" });
        }
    }
}
