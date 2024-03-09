using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Colors",
                newName: "NameAR");

            migrationBuilder.AddColumn<string>(
                name: "NameEN",
                table: "Colors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEN",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "NameAR",
                table: "Colors",
                newName: "Name");
        }
    }
}
