using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactory_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "secret",
                newName: "iv");

            migrationBuilder.AddColumn<byte[]>(
                name: "cipher_password",
                table: "secret",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "secret",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cipher_password",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "website",
                table: "secret");

            migrationBuilder.RenameColumn(
                name: "iv",
                table: "secret",
                newName: "password");
        }
    }
}
