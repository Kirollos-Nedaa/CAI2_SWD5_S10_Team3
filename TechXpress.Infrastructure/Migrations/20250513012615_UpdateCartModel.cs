using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechXpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_Customer_Id",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_Customer_Id",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Customer_Id",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Cart_Item_Id",
                table: "Cart_Items",
                newName: "CartItem_Id");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "CartItem_Id",
                table: "Cart_Items",
                newName: "Cart_Item_Id");

            migrationBuilder.AddColumn<int>(
                name: "Customer_Id",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Customer_Id",
                table: "Carts",
                column: "Customer_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_Customer_Id",
                table: "Carts",
                column: "Customer_Id",
                principalTable: "Customers",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
