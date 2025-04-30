using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyBlog.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Technology" },
                    { 2, null, "Health" },
                    { 3, null, "Sport" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedAt", "Image", "Title" },
                values: new object[,]
                {
                    { 1, 1, "C# is a programming language", new DateTime(2021, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1.png", "C#" },
                    { 2, 1, "Java is a programming language", new DateTime(2021, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1.png", "Java" },
                    { 3, 1, "Python is a programming language", new DateTime(2021, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1.png", "Python" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CommentDate", "Content", "PostId", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good", 1, "Ali" },
                    { 2, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Very Good", 1, "Reza" },
                    { 3, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nice", 1, "Mina" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
