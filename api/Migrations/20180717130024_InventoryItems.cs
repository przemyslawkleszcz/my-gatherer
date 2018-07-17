using Microsoft.EntityFrameworkCore.Migrations;

namespace mygathererapi.Migrations
{
    public partial class InventoryItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_InventoryItems_CardItems_Id",
                        column: x => x.Id,
                        principalTable: "CardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_Id",
                table: "InventoryItems",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_Cards_CardItems_Id",
                        column: x => x.Id,
                        principalTable: "CardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Id",
                table: "Cards",
                column: "Id");
        }
    }
}
