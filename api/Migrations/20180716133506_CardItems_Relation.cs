using Microsoft.EntityFrameworkCore.Migrations;

namespace mygathererapi.Migrations
{
    public partial class CardItems_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cards_Id",
                table: "Cards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardItems_Id",
                table: "Cards",
                column: "Id",
                principalTable: "CardItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardItems_Id",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_Id",
                table: "Cards");
        }
    }
}
