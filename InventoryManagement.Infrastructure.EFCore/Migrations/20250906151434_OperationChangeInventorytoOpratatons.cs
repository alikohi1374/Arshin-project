using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Infrastructure.EFCore.Migrations
{
    public partial class OperationChangeInventorytoOpratatons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_operations_Inventory_InventoryId",
                table: "operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_operations",
                table: "operations");

            migrationBuilder.RenameTable(
                name: "operations",
                newName: "InventoryOperations");

            migrationBuilder.RenameIndex(
                name: "IX_operations_InventoryId",
                table: "InventoryOperations",
                newName: "IX_InventoryOperations_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryOperations",
                table: "InventoryOperations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryOperations_Inventory_InventoryId",
                table: "InventoryOperations",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryOperations_Inventory_InventoryId",
                table: "InventoryOperations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryOperations",
                table: "InventoryOperations");

            migrationBuilder.RenameTable(
                name: "InventoryOperations",
                newName: "operations");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryOperations_InventoryId",
                table: "operations",
                newName: "IX_operations_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_operations",
                table: "operations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_operations_Inventory_InventoryId",
                table: "operations",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
