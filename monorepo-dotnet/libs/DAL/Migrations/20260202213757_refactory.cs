using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "master_password_salt",
                table: "user");

            migrationBuilder.AddColumn<byte[]>(
                name: "key_salt",
                table: "secret_key",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key_salt",
                table: "secret_key");

            migrationBuilder.AddColumn<byte[]>(
                name: "master_password_salt",
                table: "user",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
