using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class AlterForeignKeyDishToSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Sizes_SizeName1",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_SizeName1",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "SizeName1",
                table: "Dishes");

            migrationBuilder.AlterColumn<string>(
                name: "SizeName",
                table: "Dishes",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_SizeName",
                table: "Dishes",
                column: "SizeName");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Sizes_SizeName",
                table: "Dishes",
                column: "SizeName",
                principalTable: "Sizes",
                principalColumn: "SizeName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Sizes_SizeName",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_SizeName",
                table: "Dishes");

            migrationBuilder.AlterColumn<string>(
                name: "SizeName",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeName1",
                table: "Dishes",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_SizeName1",
                table: "Dishes",
                column: "SizeName1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Sizes_SizeName1",
                table: "Dishes",
                column: "SizeName1",
                principalTable: "Sizes",
                principalColumn: "SizeName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
