using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityNameFromNotfcation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityTypeEnum",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "Notifications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "EntityTypeEnum",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
