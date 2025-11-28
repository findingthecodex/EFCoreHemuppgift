using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreHemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class OrderService4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderRows",
                newName: "OrderQuantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderQuantity",
                table: "OrderRows",
                newName: "Quantity");
        }
    }
}
