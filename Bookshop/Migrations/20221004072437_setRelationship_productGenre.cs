using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class setRelationship_productGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ProductGenre",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    GenreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGenre", x => new { x.ProductId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ProductGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGenre_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGenre_GenreId",
                table: "ProductGenre",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGenre");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Genres",
                type: "int",
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
                onDelete: ReferentialAction.SetNull);
        }
    }
}
