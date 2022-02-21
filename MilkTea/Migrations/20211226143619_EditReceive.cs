using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class EditReceive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receives_Users_UserId",
                table: "Receives");

            migrationBuilder.DropIndex(
                name: "IX_Receives_UserId",
                table: "Receives");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Receives");

            migrationBuilder.RenameColumn(
                name: "ReceivePhone",
                table: "Receives",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "ReceiveNote",
                table: "Receives",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "ReceiveName",
                table: "Receives",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "ReceiveAddress",
                table: "Receives",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "OrderNote",
                table: "Orders",
                newName: "Note");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Receives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Receives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Receives",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Receives");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Receives");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Receives");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Receives",
                newName: "ReceivePhone");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Receives",
                newName: "ReceiveNote");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Receives",
                newName: "ReceiveName");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Receives",
                newName: "ReceiveAddress");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Orders",
                newName: "OrderNote");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Receives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Receives_UserId",
                table: "Receives",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receives_Users_UserId",
                table: "Receives",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
