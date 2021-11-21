using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTea.Migrations
{
    public partial class AlterConstraintOrderRecaive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ReceiveId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiveId",
                table: "Orders",
                column: "ReceiveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ReceiveId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiveId",
                table: "Orders",
                column: "ReceiveId",
                unique: true);
        }
    }
}
