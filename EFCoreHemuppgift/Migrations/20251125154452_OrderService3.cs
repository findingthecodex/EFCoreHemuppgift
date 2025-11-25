using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreHemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class OrderService3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "ProductPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                table: "Products",
                newName: "Price");
        }
    }
}
