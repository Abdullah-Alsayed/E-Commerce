using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "whatsapp",
                table: "Settings",
                newName: "Whatsapp");

            migrationBuilder.RenameColumn(
                name: "Reviews",
                table: "Reviews",
                newName: "Review");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Whatsapp",
                table: "Settings",
                newName: "whatsapp");

            migrationBuilder.RenameColumn(
                name: "Review",
                table: "Reviews",
                newName: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Notifications",
                type: "text",
                nullable: true);
        }
    }
}
