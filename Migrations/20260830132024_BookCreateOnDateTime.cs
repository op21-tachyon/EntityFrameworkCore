using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class BookCreateOnDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedOn",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CreatedOn", "Description", "Title", "isActive" },
                values: new object[,]
                {
                    { 1, "30-08-2026 13:00:30", "Description for Harry Potter 1", "Harry Potter 1", "true" },
                    { 2, "30-08-2026 13:00:30", "Description for Harry Potter 2", "Harry Potter 2", "true" },
                    { 3, "30-08-2026 13:00:30", "Description for Harry Potter 3", "Harry Potter 3", "true" }
                });
        }
    }
}
