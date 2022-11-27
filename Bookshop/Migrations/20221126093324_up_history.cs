using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class up_history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Veriry",
                table: "PurchaseHistories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Veriry",
                table: "PurchaseHistories");
        }
    }
}
