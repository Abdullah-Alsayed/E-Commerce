using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpadteNotifactios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "operationTypeEnum",
                table: "Notifications",
                newName: "OperationType");

            migrationBuilder.AlterColumn<string>(
                name: "MessageEN",
                table: "Notifications",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Entity",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entity",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "Notifications",
                newName: "operationTypeEnum");

            migrationBuilder.AlterColumn<string>(
                name: "MessageEN",
                table: "Notifications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "Notifications",
                type: "text",
                nullable: true);
        }
    }
}
