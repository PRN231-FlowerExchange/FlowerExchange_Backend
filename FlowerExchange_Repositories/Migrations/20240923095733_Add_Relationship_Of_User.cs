using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowerExchange_Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Add_Relationship_Of_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "Report",
                newName: "UpdateById");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "Report",
                newName: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_BuyerId",
                table: "ServiceOrder",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_CreateById",
                table: "Report",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Report_UpdateById",
                table: "Report",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerOrder_BuyerId",
                table: "FlowerOrder",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerOrder_SellerId",
                table: "FlowerOrder",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerOrder_User_BuyerId",
                table: "FlowerOrder",
                column: "BuyerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerOrder_User_SellerId",
                table: "FlowerOrder",
                column: "SellerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_CreateById",
                table: "Report",
                column: "CreateById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_UpdateById",
                table: "Report",
                column: "UpdateById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrder_User_BuyerId",
                table: "ServiceOrder",
                column: "BuyerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowerOrder_User_BuyerId",
                table: "FlowerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowerOrder_User_SellerId",
                table: "FlowerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_CreateById",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_UpdateById",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrder_User_BuyerId",
                table: "ServiceOrder");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrder_BuyerId",
                table: "ServiceOrder");

            migrationBuilder.DropIndex(
                name: "IX_Report_CreateById",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_UpdateById",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_FlowerOrder_BuyerId",
                table: "FlowerOrder");

            migrationBuilder.DropIndex(
                name: "IX_FlowerOrder_SellerId",
                table: "FlowerOrder");

            migrationBuilder.RenameColumn(
                name: "UpdateById",
                table: "Report",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreateById",
                table: "Report",
                newName: "CreateBy");
        }
    }
}
