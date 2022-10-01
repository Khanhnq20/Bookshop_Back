using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class form_file : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pages",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fileImage",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Formats",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileImage",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Pages",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Formats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
