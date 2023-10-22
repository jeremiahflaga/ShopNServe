using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNServe.ProductCatalog.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriceFromDecimalToMoneyValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Product",
                newName: "Price_Amount");

            migrationBuilder.AddColumn<string>(
                name: "Price_Currency",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price_Currency",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "Product",
                newName: "Price");
        }
    }
}
