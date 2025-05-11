using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechXpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrandsTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Customers_Customer_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Items_Cart_Cart_Id",
                table: "Cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Item_Discount_Discount_Id",
                table: "Discount_Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Item_Products_Product_Id",
                table: "Discount_Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Item_Orders_Order_Id",
                table: "Order_Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Item_Products_Product_Id",
                table: "Order_Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Orders_Oreder_Id",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_Brand_Id",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Item",
                table: "Order_Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount_Item",
                table: "Discount_Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Order_Item",
                newName: "Order_Items");

            migrationBuilder.RenameTable(
                name: "Discount_Item",
                newName: "Discount_Items");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "Discounts");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_Oreder_Id",
                table: "Payments",
                newName: "IX_Payments_Oreder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Item_Product_Id",
                table: "Order_Items",
                newName: "IX_Order_Items_Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Item_Order_Id",
                table: "Order_Items",
                newName: "IX_Order_Items_Order_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_Item_Product_Id",
                table: "Discount_Items",
                newName: "IX_Discount_Items_Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_Item_Discount_Id",
                table: "Discount_Items",
                newName: "IX_Discount_Items_Discount_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Customer_Id",
                table: "Carts",
                newName: "IX_Carts_Customer_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Payment_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Items",
                table: "Order_Items",
                column: "Order_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount_Items",
                table: "Discount_Items",
                column: "Discount_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "Discount_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Cart_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Brand_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Items_Carts_Cart_Id",
                table: "Cart_Items",
                column: "Cart_Id",
                principalTable: "Carts",
                principalColumn: "Cart_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_Customer_Id",
                table: "Carts",
                column: "Customer_Id",
                principalTable: "Customers",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Items_Discounts_Discount_Id",
                table: "Discount_Items",
                column: "Discount_Id",
                principalTable: "Discounts",
                principalColumn: "Discount_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Items_Products_Product_Id",
                table: "Discount_Items",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Orders_Order_Id",
                table: "Order_Items",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Products_Product_Id",
                table: "Order_Items",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_Oreder_Id",
                table: "Payments",
                column: "Oreder_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_Brand_Id",
                table: "Products",
                column: "Brand_Id",
                principalTable: "Brands",
                principalColumn: "Brand_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Items_Carts_Cart_Id",
                table: "Cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_Customer_Id",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Items_Discounts_Discount_Id",
                table: "Discount_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Items_Products_Product_Id",
                table: "Discount_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Orders_Order_Id",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Products_Product_Id",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_Oreder_Id",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_Brand_Id",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Items",
                table: "Order_Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount_Items",
                table: "Discount_Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Order_Items",
                newName: "Order_Item");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount_Items",
                newName: "Discount_Item");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Cart");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_Oreder_Id",
                table: "Payment",
                newName: "IX_Payment_Oreder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Items_Product_Id",
                table: "Order_Item",
                newName: "IX_Order_Item_Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Items_Order_Id",
                table: "Order_Item",
                newName: "IX_Order_Item_Order_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_Items_Product_Id",
                table: "Discount_Item",
                newName: "IX_Discount_Item_Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_Items_Discount_Id",
                table: "Discount_Item",
                newName: "IX_Discount_Item_Discount_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_Customer_Id",
                table: "Cart",
                newName: "IX_Cart_Customer_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Payment_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Item",
                table: "Order_Item",
                column: "Order_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Discount_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount_Item",
                table: "Discount_Item",
                column: "Discount_Item_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "Cart_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "Brand_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Customers_Customer_Id",
                table: "Cart",
                column: "Customer_Id",
                principalTable: "Customers",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Items_Cart_Cart_Id",
                table: "Cart_Items",
                column: "Cart_Id",
                principalTable: "Cart",
                principalColumn: "Cart_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Item_Discount_Discount_Id",
                table: "Discount_Item",
                column: "Discount_Id",
                principalTable: "Discount",
                principalColumn: "Discount_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Item_Products_Product_Id",
                table: "Discount_Item",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Item_Orders_Order_Id",
                table: "Order_Item",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Item_Products_Product_Id",
                table: "Order_Item",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Orders_Oreder_Id",
                table: "Payment",
                column: "Oreder_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_Brand_Id",
                table: "Products",
                column: "Brand_Id",
                principalTable: "Brand",
                principalColumn: "Brand_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
