using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class up_history2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Veriry",
                table: "PurchaseHistories");

            migrationBuilder.AddColumn<bool>(
                name: "Verify",
                table: "PurchaseHistories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verify",
                table: "PurchaseHistories");

            migrationBuilder.AddColumn<bool>(
                name: "Veriry",
                table: "PurchaseHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
