using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_AspNetUsers_CreateBy",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Area_Governorate_GovernorateID",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Brand_AspNetUsers_CreateBy",
                table: "Brand");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_CreateBy",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Color_AspNetUsers_CreateBy",
                table: "Color");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AspNetUsers_CreateBy",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_AspNetUsers_CreateBy",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Product_ProductID",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Governorate_AspNetUsers_CreateBy",
                table: "Governorate");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_CreateBy",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Area_AreaID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Governorate_GovernorateID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_PromoCode_PromoCodeID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Status_StatusID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_CreateBy",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Brand_BrandID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubCategory_SubCategoryID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Unit_UnitID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Color_ColorId",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Product_ProductId",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Order_OrderID",
                table: "ProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Product_ProductID",
                table: "ProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_AspNetUsers_CreateBy",
                table: "ProductPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_Product_ProductID",
                table: "ProductPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Product_ProductID",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_AspNetUsers_CreateBy",
                table: "PromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_CreateBy",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Product_ProductID",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Setting_SettingID",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_AspNetUsers_CreateBy",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderPhoto_AspNetUsers_CreateBy",
                table: "SliderPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_Status_AspNetUsers_CreateBy",
                table: "Status");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_AspNetUsers_CreateBy",
                table: "SubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_Category_CategoryID",
                table: "SubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Unit_AspNetUsers_CreateBy",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategory",
                table: "SubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderPhoto",
                table: "SliderPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Setting",
                table: "Setting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPhoto",
                table: "ProductPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOrder",
                table: "ProductOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Governorate",
                table: "Governorate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Color",
                table: "Color");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Area",
                table: "Area");

            migrationBuilder.RenameTable(
                name: "Unit",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "SubCategory",
                newName: "SubCategories");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "SliderPhoto",
                newName: "SliderPhotos");

            migrationBuilder.RenameTable(
                name: "Setting",
                newName: "Settings");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "PromoCode",
                newName: "PromoCodes");

            migrationBuilder.RenameTable(
                name: "ProductPhoto",
                newName: "productPhotos");

            migrationBuilder.RenameTable(
                name: "ProductOrder",
                newName: "ProductOrders");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Governorate",
                newName: "Governorates");

            migrationBuilder.RenameTable(
                name: "Favorite",
                newName: "Favorites");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameTable(
                name: "Color",
                newName: "Colors");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameTable(
                name: "Area",
                newName: "Areas");

            migrationBuilder.RenameIndex(
                name: "IX_Unit_CreateBy",
                table: "Units",
                newName: "IX_Units_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategory_CreateBy",
                table: "SubCategories",
                newName: "IX_SubCategories_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategory_CategoryID",
                table: "SubCategories",
                newName: "IX_SubCategories_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Status_CreateBy",
                table: "Statuses",
                newName: "IX_Statuses_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SliderPhoto_CreateBy",
                table: "SliderPhotos",
                newName: "IX_SliderPhotos_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_CreateBy",
                table: "Settings",
                newName: "IX_Settings_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProductID",
                table: "Reviews",
                newName: "IX_Reviews_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CreateBy",
                table: "Reviews",
                newName: "IX_Reviews_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCode_CreateBy",
                table: "PromoCodes",
                newName: "IX_PromoCodes_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_ProductID",
                table: "productPhotos",
                newName: "IX_productPhotos_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_CreateBy",
                table: "productPhotos",
                newName: "IX_productPhotos_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrder_ProductID",
                table: "ProductOrders",
                newName: "IX_ProductOrders_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrder_OrderID",
                table: "ProductOrders",
                newName: "IX_ProductOrders_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_UnitID",
                table: "Products",
                newName: "IX_Products_UnitID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SubCategoryID",
                table: "Products",
                newName: "IX_Products_SubCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CreateBy",
                table: "Products",
                newName: "IX_Products_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_BrandID",
                table: "Products",
                newName: "IX_Products_BrandID");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_CreateBy",
                table: "Notifications",
                newName: "IX_Notifications_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Governorate_CreateBy",
                table: "Governorates",
                newName: "IX_Governorates_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_ProductID",
                table: "Favorites",
                newName: "IX_Favorites_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_CreateBy",
                table: "Favorites",
                newName: "IX_Favorites_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_CreateBy",
                table: "Expenses",
                newName: "IX_Expenses_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Color_CreateBy",
                table: "Colors",
                newName: "IX_Colors_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Category_CreateBy",
                table: "Categories",
                newName: "IX_Categories_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brand_CreateBy",
                table: "Brands",
                newName: "IX_Brands_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Area_GovernorateID",
                table: "Areas",
                newName: "IX_Areas_GovernorateID");

            migrationBuilder.RenameIndex(
                name: "IX_Area_CreateBy",
                table: "Areas",
                newName: "IX_Areas_CreateBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderPhotos",
                table: "SliderPhotos",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productPhotos",
                table: "productPhotos",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrders",
                table: "ProductOrders",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Governorates",
                table: "Governorates",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Areas",
                table: "Areas",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreateBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ModifyBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContactUs_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    CreateBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ModifyBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_CreateBy",
                table: "ContactUs",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CreateBy",
                table: "Feedbacks",
                column: "CreateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_AspNetUsers_CreateBy",
                table: "Areas",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Governorates_GovernorateID",
                table: "Areas",
                column: "GovernorateID",
                principalTable: "Governorates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_CreateBy",
                table: "Brands",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreateBy",
                table: "Categories",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_AspNetUsers_CreateBy",
                table: "Colors",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_CreateBy",
                table: "Expenses",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_CreateBy",
                table: "Favorites",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Products_ProductID",
                table: "Favorites",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Governorates_AspNetUsers_CreateBy",
                table: "Governorates",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_CreateBy",
                table: "Notifications",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Areas_AreaID",
                table: "Order",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Governorates_GovernorateID",
                table: "Order",
                column: "GovernorateID",
                principalTable: "Governorates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PromoCodes_PromoCodeID",
                table: "Order",
                column: "PromoCodeID",
                principalTable: "PromoCodes",
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
                name: "FK_ProductColor_Colors_ColorId",
                table: "ProductColor",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Products_ProductId",
                table: "ProductColor",
                column: "ProductId",
                principalTable: "Products",
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
                name: "FK_ProductOrders_Products_ProductID",
                table: "ProductOrders",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productPhotos_AspNetUsers_CreateBy",
                table: "productPhotos",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productPhotos_Products_ProductID",
                table: "productPhotos",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreateBy",
                table: "Products",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryID",
                table: "Products",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitID",
                table: "Products",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Products_ProductID",
                table: "ProductStock",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_AspNetUsers_CreateBy",
                table: "PromoCodes",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_CreateBy",
                table: "Reviews",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Settings_SettingID",
                table: "Section",
                column: "SettingID",
                principalTable: "Settings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_AspNetUsers_CreateBy",
                table: "Settings",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SliderPhotos_AspNetUsers_CreateBy",
                table: "SliderPhotos",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_AspNetUsers_CreateBy",
                table: "Statuses",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreateBy",
                table: "SubCategories",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_CategoryID",
                table: "SubCategories",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_CreateBy",
                table: "Units",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_AspNetUsers_CreateBy",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Governorates_GovernorateID",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_CreateBy",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreateBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_AspNetUsers_CreateBy",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_CreateBy",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_CreateBy",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Products_ProductID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Governorates_AspNetUsers_CreateBy",
                table: "Governorates");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_CreateBy",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Areas_AreaID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Governorates_GovernorateID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_PromoCodes_PromoCodeID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Statuses_StatusID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Colors_ColorId",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Products_ProductId",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Order_OrderID",
                table: "ProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Products_ProductID",
                table: "ProductOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_productPhotos_AspNetUsers_CreateBy",
                table: "productPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_productPhotos_Products_ProductID",
                table: "productPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreateBy",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Products_ProductID",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_AspNetUsers_CreateBy",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_CreateBy",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Settings_SettingID",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_AspNetUsers_CreateBy",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderPhotos_AspNetUsers_CreateBy",
                table: "SliderPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_AspNetUsers_CreateBy",
                table: "Statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreateBy",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_CategoryID",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_CreateBy",
                table: "Units");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderPhotos",
                table: "SliderPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productPhotos",
                table: "productPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOrders",
                table: "ProductOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Governorates",
                table: "Governorates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Areas",
                table: "Areas");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Unit");

            migrationBuilder.RenameTable(
                name: "SubCategories",
                newName: "SubCategory");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "SliderPhotos",
                newName: "SliderPhoto");

            migrationBuilder.RenameTable(
                name: "Settings",
                newName: "Setting");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "PromoCodes",
                newName: "PromoCode");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "productPhotos",
                newName: "ProductPhoto");

            migrationBuilder.RenameTable(
                name: "ProductOrders",
                newName: "ProductOrder");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Governorates",
                newName: "Governorate");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "Favorite");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Color");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameTable(
                name: "Areas",
                newName: "Area");

            migrationBuilder.RenameIndex(
                name: "IX_Units_CreateBy",
                table: "Unit",
                newName: "IX_Unit_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CreateBy",
                table: "SubCategory",
                newName: "IX_SubCategory_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CategoryID",
                table: "SubCategory",
                newName: "IX_SubCategory_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Statuses_CreateBy",
                table: "Status",
                newName: "IX_Status_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_SliderPhotos_CreateBy",
                table: "SliderPhoto",
                newName: "IX_SliderPhoto_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_CreateBy",
                table: "Setting",
                newName: "IX_Setting_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductID",
                table: "Review",
                newName: "IX_Review_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CreateBy",
                table: "Review",
                newName: "IX_Review_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodes_CreateBy",
                table: "PromoCode",
                newName: "IX_PromoCode_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UnitID",
                table: "Product",
                newName: "IX_Product_UnitID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubCategoryID",
                table: "Product",
                newName: "IX_Product_SubCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreateBy",
                table: "Product",
                newName: "IX_Product_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandID",
                table: "Product",
                newName: "IX_Product_BrandID");

            migrationBuilder.RenameIndex(
                name: "IX_productPhotos_ProductID",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_productPhotos_CreateBy",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrders_ProductID",
                table: "ProductOrder",
                newName: "IX_ProductOrder_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrders_OrderID",
                table: "ProductOrder",
                newName: "IX_ProductOrder_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CreateBy",
                table: "Notification",
                newName: "IX_Notification_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Governorates_CreateBy",
                table: "Governorate",
                newName: "IX_Governorate_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_ProductID",
                table: "Favorite",
                newName: "IX_Favorite_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_CreateBy",
                table: "Favorite",
                newName: "IX_Favorite_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_CreateBy",
                table: "Expense",
                newName: "IX_Expense_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Colors_CreateBy",
                table: "Color",
                newName: "IX_Color_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CreateBy",
                table: "Category",
                newName: "IX_Category_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_CreateBy",
                table: "Brand",
                newName: "IX_Brand_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_GovernorateID",
                table: "Area",
                newName: "IX_Area_GovernorateID");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_CreateBy",
                table: "Area",
                newName: "IX_Area_CreateBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                table: "Unit",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategory",
                table: "SubCategory",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderPhoto",
                table: "SliderPhoto",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Setting",
                table: "Setting",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPhoto",
                table: "ProductPhoto",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrder",
                table: "ProductOrder",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Governorate",
                table: "Governorate",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Color",
                table: "Color",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Area",
                table: "Area",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_AspNetUsers_CreateBy",
                table: "Area",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Governorate_GovernorateID",
                table: "Area",
                column: "GovernorateID",
                principalTable: "Governorate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_AspNetUsers_CreateBy",
                table: "Brand",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_CreateBy",
                table: "Category",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Color_AspNetUsers_CreateBy",
                table: "Color",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AspNetUsers_CreateBy",
                table: "Expense",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_AspNetUsers_CreateBy",
                table: "Favorite",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Product_ProductID",
                table: "Favorite",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Governorate_AspNetUsers_CreateBy",
                table: "Governorate",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_CreateBy",
                table: "Notification",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Area_AreaID",
                table: "Order",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Governorate_GovernorateID",
                table: "Order",
                column: "GovernorateID",
                principalTable: "Governorate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PromoCode_PromoCodeID",
                table: "Order",
                column: "PromoCodeID",
                principalTable: "PromoCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Status_StatusID",
                table: "Order",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_CreateBy",
                table: "Product",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Brand_BrandID",
                table: "Product",
                column: "BrandID",
                principalTable: "Brand",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubCategory_SubCategoryID",
                table: "Product",
                column: "SubCategoryID",
                principalTable: "SubCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Unit_UnitID",
                table: "Product",
                column: "UnitID",
                principalTable: "Unit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Color_ColorId",
                table: "ProductColor",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Product_ProductId",
                table: "ProductColor",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Order_OrderID",
                table: "ProductOrder",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Product_ProductID",
                table: "ProductOrder",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_AspNetUsers_CreateBy",
                table: "ProductPhoto",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_Product_ProductID",
                table: "ProductPhoto",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Product_ProductID",
                table: "ProductStock",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_AspNetUsers_CreateBy",
                table: "PromoCode",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_CreateBy",
                table: "Review",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Product_ProductID",
                table: "Review",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Setting_SettingID",
                table: "Section",
                column: "SettingID",
                principalTable: "Setting",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_AspNetUsers_CreateBy",
                table: "Setting",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SliderPhoto_AspNetUsers_CreateBy",
                table: "SliderPhoto",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Status_AspNetUsers_CreateBy",
                table: "Status",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_AspNetUsers_CreateBy",
                table: "SubCategory",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_Category_CategoryID",
                table: "SubCategory",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_AspNetUsers_CreateBy",
                table: "Unit",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
