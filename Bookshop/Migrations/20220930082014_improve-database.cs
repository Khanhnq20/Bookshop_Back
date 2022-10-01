using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class improvedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Genres",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Genres",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Products_ProductId",
                table: "Genres",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
