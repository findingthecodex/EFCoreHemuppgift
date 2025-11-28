using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreHemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class CustomerService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customers",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customers",
                newName: "CustomerEmail");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Customers",
                newName: "CustomerCity");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                newName: "IX_Customers_CustomerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Customers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "Customers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "CustomerCity",
                table: "Customers",
                newName: "City");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CustomerEmail",
                table: "Customers",
                newName: "IX_Customers_Email");
        }
    }
}
