using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNServe.ProductCatalog.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Products",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");
        }
    }
}
