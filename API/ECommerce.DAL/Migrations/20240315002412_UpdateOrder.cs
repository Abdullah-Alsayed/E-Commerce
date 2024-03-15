using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Areas_AreaID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_CreateBy",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Governorates_GovernorateID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Statuses_StatusID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Voucher_PromoCodeID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Order_OrderID",
                table: "ProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_AspNetUsers_CreateBy",
                table: "Voucher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PromoCodeID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PromoCodeID",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Voucher",
                newName: "Vouchers");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Voucher_CreateBy",
                table: "Vouchers",
                newName: "IX_Vouchers_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Order_StatusID",
                table: "Orders",
                newName: "IX_Orders_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_GovernorateID",
                table: "Orders",
                newName: "IX_Orders_GovernorateID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CreateBy",
                table: "Orders",
                newName: "IX_Orders_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Order_AreaID",
                table: "Orders",
                newName: "IX_Orders_AreaID");

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherID",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Areas_AreaID",
                table: "Orders",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CreateBy",
                table: "Orders",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Governorates_GovernorateID",
                table: "Orders",
                column: "GovernorateID",
                principalTable: "Governorates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusID",
                table: "Orders",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders",
                column: "VoucherID",
                principalTable: "Vouchers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Orders_OrderID",
                table: "ProductOrders",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_AspNetUsers_CreateBy",
                table: "Vouchers",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Areas_AreaID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CreateBy",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Governorates_GovernorateID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Orders_OrderID",
                table: "ProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_AspNetUsers_CreateBy",
                table: "Vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Vouchers",
                newName: "Voucher");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Vouchers_CreateBy",
                table: "Voucher",
                newName: "IX_Voucher_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_StatusID",
                table: "Order",
                newName: "IX_Order_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_GovernorateID",
                table: "Order",
                newName: "IX_Order_GovernorateID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CreateBy",
                table: "Order",
                newName: "IX_Order_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AreaID",
                table: "Order",
                newName: "IX_Order_AreaID");

            migrationBuilder.AddColumn<Guid>(
                name: "PromoCodeID",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PromoCodeID",
                table: "Order",
                column: "PromoCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Areas_AreaID",
                table: "Order",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_CreateBy",
                table: "Order",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Governorates_GovernorateID",
                table: "Order",
                column: "GovernorateID",
                principalTable: "Governorates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Statuses_StatusID",
                table: "Order",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Voucher_PromoCodeID",
                table: "Order",
                column: "PromoCodeID",
                principalTable: "Voucher",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Order_OrderID",
                table: "ProductOrders",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_AspNetUsers_CreateBy",
                table: "Voucher",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
