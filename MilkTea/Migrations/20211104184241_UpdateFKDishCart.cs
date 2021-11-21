using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class UpdateFKDishCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Orders_CartOrderId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CartOrderId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CartOrderId",
                table: "Dishes");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartOrderId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CartOrderId",
                table: "Dishes",
                column: "CartOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Orders_CartOrderId",
                table: "Dishes",
                column: "CartOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
