using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class comment_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAth",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EmailAth",
                table: "Comments");
        }
    }
}
