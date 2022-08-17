using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class updateproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Formats_BookId",
                table: "Formats");

            migrationBuilder.CreateIndex(
                name: "IX_Formats_BookId",
                table: "Formats",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Formats_BookId",
                table: "Formats");

            migrationBuilder.CreateIndex(
                name: "IX_Formats_BookId",
                table: "Formats",
                column: "BookId",
                unique: true);
        }
    }
}
