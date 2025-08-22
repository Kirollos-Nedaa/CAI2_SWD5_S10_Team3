using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechXpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RefreshTokens",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "RefreshTokens",
                newName: "CreatedAt");
        }
    }
}
