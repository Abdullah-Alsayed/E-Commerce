using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVoucherEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PromoCodes_PromoCodeID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Voucher", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Voucher_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_CreateBy",
                table: "Voucher",
                column: "CreateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Voucher_PromoCodeID",
                table: "Order",
                column: "PromoCodeID",
                principalTable: "Voucher",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Voucher_PromoCodeID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifyBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PromoCodes_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_CreateBy",
                table: "PromoCodes",
                column: "CreateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PromoCodes_PromoCodeID",
                table: "Order",
                column: "PromoCodeID",
                principalTable: "PromoCodes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
