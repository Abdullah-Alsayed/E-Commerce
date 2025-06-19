using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CollectionsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_Colors_ColorId",
                table: "ProductPhoto");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "ProductPhoto",
                newName: "ColorID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_ColorId",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_ColorID");

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Rules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 450, nullable: false),
                    ModifyBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 450, nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 450, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCollection",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductCollection_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCollection_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collections_CreateBy",
                table: "Collections",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCollection_CollectionID",
                table: "ProductCollection",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCollection_ProductID",
                table: "ProductCollection",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_Colors_ColorID",
                table: "ProductPhoto",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_Colors_ColorID",
                table: "ProductPhoto");

            migrationBuilder.DropTable(
                name: "ProductCollection");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.RenameColumn(
                name: "ColorID",
                table: "ProductPhoto",
                newName: "ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_ColorID",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_Colors_ColorId",
                table: "ProductPhoto",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");
        }
    }
}
