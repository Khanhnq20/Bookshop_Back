using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookshop.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGenre_Genres_GenreId",
                table: "ProductGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGenre_Products_ProductId",
                table: "ProductGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGenre",
                table: "ProductGenre");

            migrationBuilder.RenameTable(
                name: "ProductGenre",
                newName: "ProductGenres");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGenre_GenreId",
                table: "ProductGenres",
                newName: "IX_ProductGenres_GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGenres",
                table: "ProductGenres",
                columns: new[] { "ProductId", "GenreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGenres_Genres_GenreId",
                table: "ProductGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGenres_Products_ProductId",
                table: "ProductGenres",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGenres_Genres_GenreId",
                table: "ProductGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGenres_Products_ProductId",
                table: "ProductGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGenres",
                table: "ProductGenres");

            migrationBuilder.RenameTable(
                name: "ProductGenres",
                newName: "ProductGenre");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGenres_GenreId",
                table: "ProductGenre",
                newName: "IX_ProductGenre_GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGenre",
                table: "ProductGenre",
                columns: new[] { "ProductId", "GenreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGenre_Genres_GenreId",
                table: "ProductGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGenre_Products_ProductId",
                table: "ProductGenre",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
