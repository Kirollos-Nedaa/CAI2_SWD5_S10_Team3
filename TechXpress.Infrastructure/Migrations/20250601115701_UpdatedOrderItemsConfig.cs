using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechXpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOrderItemsConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Orders_OrdersOrder_Id",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_OrdersOrder_Id",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "OrdersOrder_Id",
                table: "Order_Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdersOrder_Id",
                table: "Order_Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_OrdersOrder_Id",
                table: "Order_Items",
                column: "OrdersOrder_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Orders_OrdersOrder_Id",
                table: "Order_Items",
                column: "OrdersOrder_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id");
        }
    }
}
