using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreHemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class OrderService2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "ProductDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductDescription",
                table: "Products",
                newName: "Description");
        }
    }
}
