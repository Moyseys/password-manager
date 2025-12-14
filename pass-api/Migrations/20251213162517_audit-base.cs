using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class auditbase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "secret_key",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "secret_key",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "secret_key",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "secret_key",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "secret_key",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "secret_key",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                table: "secret_key",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "secret",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "secret",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "secret",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "secret",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "secret",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "secret",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                table: "secret",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "secret_key");

            migrationBuilder.DropColumn(
                name: "active",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "secret");
        }
    }
}
