using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.DAL.Migrations
{
    public partial class UpdateProdactImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrateDate",
                table: "ProdactImgs");

            migrationBuilder.AddColumn<DateTime>(
                name: "CrateAt",
                table: "ProdactImgs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrateAt",
                table: "ProdactImgs");

            migrationBuilder.AddColumn<int>(
                name: "CrateDate",
                table: "ProdactImgs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
