using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class secretkeyv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "key_salt",
                table: "secret_key",
                newName: "salt");

            migrationBuilder.AddColumn<string>(
                name: "algorithm",
                table: "secret_key",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "derivation_algorithm",
                table: "secret_key",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "hash_algorithm",
                table: "secret_key",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "iterations",
                table: "secret_key",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "key_iv",
                table: "secret_key",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "key_size",
                table: "secret_key",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "salt_size",
                table: "secret_key",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "algorithm",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "derivation_algorithm",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "hash_algorithm",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "iterations",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "key_iv",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "key_size",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "salt_size",
                table: "secret_key");

            migrationBuilder.RenameColumn(
                name: "salt",
                table: "secret_key",
                newName: "key_salt");
        }
    }
}
