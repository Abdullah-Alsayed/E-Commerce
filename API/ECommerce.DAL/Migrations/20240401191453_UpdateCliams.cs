using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCliams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserClaims",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "AspNetUserClaims",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "AspNetUserClaims",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoleClaims",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "AspNetRoleClaims",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "AspNetRoleClaims",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "AspNetRoleClaims");
        }
    }
}
