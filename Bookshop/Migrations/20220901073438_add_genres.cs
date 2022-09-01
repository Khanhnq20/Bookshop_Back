using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class add_genres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Genres",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ProductId",
                table: "Genres",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_ProductId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Genres");
        }
    }
}
