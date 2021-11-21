using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class CreateDishOrderAndCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Dishes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartOrderId",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    TotolPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_Users_CartId",
                        column: x => x.CartId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CartId",
                table: "Dishes",
                column: "CartId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Carts_CartId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Orders_CartOrderId",
                table: "Dishes");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CartId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CartOrderId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CartOrderId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Dishes");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
