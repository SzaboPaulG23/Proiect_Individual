using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogOnline2.Migrations
{
    /// <inheritdoc />
    public partial class DB_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "User",
                type: "nvarchar(max)",
                defaultValue: "x",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "user_type",
                table: "User",
                type: "nvarchar(max)",
                defaultValue: "x",
                nullable: false);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "user_type",
                table: "User");

        }
    }
}
