using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CreatedOn", "Description", "Title", "isActive" },
                values: new object[,]
                {
                    { 1, "30-08-2026 12:06:13", "Description for Harry Potter 1", "Harry Potter 1", "true" },
                    { 2, "30-08-2026 12:06:13", "Description for Harry Potter 2", "Harry Potter 2", "true" },
                    { 3, "30-08-2026 12:06:13", "Description for Harry Potter 3", "Harry Potter 3", "true" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
