using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Vendor_VendorID",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_AspNetUsers_CreateBy",
                table: "Vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_CreateBy",
                table: "Vendors",
                newName: "IX_Vendors_CreateBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Vendors_VendorID",
                table: "ProductStock",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_AspNetUsers_CreateBy",
                table: "Vendors",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Vendors_VendorID",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_AspNetUsers_CreateBy",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.RenameIndex(
                name: "IX_Vendors_CreateBy",
                table: "Vendor",
                newName: "IX_Vendor_CreateBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Vendor_VendorID",
                table: "ProductStock",
                column: "VendorID",
                principalTable: "Vendor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_AspNetUsers_CreateBy",
                table: "Vendor",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
