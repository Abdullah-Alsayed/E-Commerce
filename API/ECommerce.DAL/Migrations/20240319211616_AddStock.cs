using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStock");

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductID = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorID = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreateBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ModifyBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stocks_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CreateBy",
                table: "Stocks",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductID",
                table: "Stocks",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_VendorID",
                table: "Stocks",
                column: "VendorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.CreateTable(
                name: "ProductStock",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ProductID = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifyBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStock", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductStock_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStock_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStock_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_CreateBy",
                table: "ProductStock",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_ProductID",
                table: "ProductStock",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_VendorID",
                table: "ProductStock",
                column: "VendorID");
        }
    }
}
