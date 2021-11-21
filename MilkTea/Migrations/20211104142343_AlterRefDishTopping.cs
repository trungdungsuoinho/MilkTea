using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class AlterRefDishTopping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishTopping");

            migrationBuilder.AddColumn<int>(
                name: "ToppingId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_ToppingId",
                table: "Dishes",
                column: "ToppingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Toppings_ToppingId",
                table: "Dishes",
                column: "ToppingId",
                principalTable: "Toppings",
                principalColumn: "ToppingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Toppings_ToppingId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_ToppingId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "ToppingId",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "DishTopping",
                columns: table => new
                {
                    DishsDishId = table.Column<int>(type: "int", nullable: false),
                    ToppingsToppingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTopping", x => new { x.DishsDishId, x.ToppingsToppingId });
                    table.ForeignKey(
                        name: "FK_DishTopping_Dishes_DishsDishId",
                        column: x => x.DishsDishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishTopping_Toppings_ToppingsToppingId",
                        column: x => x.ToppingsToppingId,
                        principalTable: "Toppings",
                        principalColumn: "ToppingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishTopping_ToppingsToppingId",
                table: "DishTopping",
                column: "ToppingsToppingId");
        }
    }
}
