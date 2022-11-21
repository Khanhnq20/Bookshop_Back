using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class update_purchaseProduct_last : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "PurchaseHistories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistories_UserId1",
                table: "PurchaseHistories",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistories_AspNetUsers_UserId1",
                table: "PurchaseHistories",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistories_AspNetUsers_UserId1",
                table: "PurchaseHistories");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistories_UserId1",
                table: "PurchaseHistories");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PurchaseHistories");
        }
    }
}
