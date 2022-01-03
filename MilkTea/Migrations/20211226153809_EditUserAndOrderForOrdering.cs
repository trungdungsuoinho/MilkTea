using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class EditUserAndOrderForOrdering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiveId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VerifyEmail",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VerifyPhone",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReceiveId",
                table: "Users",
                column: "ReceiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Receives_ReceiveId",
                table: "Users",
                column: "ReceiveId",
                principalTable: "Receives",
                principalColumn: "ReceiveId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Receives_ReceiveId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReceiveId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReceiveId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerifyEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerifyPhone",
                table: "Users");
        }
    }
}
