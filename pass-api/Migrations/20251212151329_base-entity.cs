using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class baseentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Secret_User_UserId",
                table: "Secret");

            migrationBuilder.DropForeignKey(
                name: "FK_SecretKey_User_UserId",
                table: "SecretKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecretKey",
                table: "SecretKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Secret",
                table: "Secret");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "SecretKey",
                newName: "secretkey");

            migrationBuilder.RenameTable(
                name: "Secret",
                newName: "secret");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "user",
                newName: "passwordhash");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "user",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "MasterPasswordSalt",
                table: "user",
                newName: "masterpasswordsalt");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "secretkey",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "secretkey",
                newName: "key");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "secretkey",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_SecretKey_UserId",
                table: "secretkey",
                newName: "IX_secretkey_userid");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "secret",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "secret",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "secret",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "secret",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "secret",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Secret_UserId",
                table: "secret",
                newName: "IX_secret_userid");

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdat",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "createdby",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletedat",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deletedby",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedat",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updatedby",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_secretkey",
                table: "secretkey",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_secret",
                table: "secret",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_secret_user_userid",
                table: "secret",
                column: "userid",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_secretkey_user_userid",
                table: "secretkey",
                column: "userid",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_secret_user_userid",
                table: "secret");

            migrationBuilder.DropForeignKey(
                name: "FK_secretkey_user_userid",
                table: "secretkey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_secretkey",
                table: "secretkey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_secret",
                table: "secret");

            migrationBuilder.DropColumn(
                name: "active",
                table: "user");

            migrationBuilder.DropColumn(
                name: "createdat",
                table: "user");

            migrationBuilder.DropColumn(
                name: "createdby",
                table: "user");

            migrationBuilder.DropColumn(
                name: "deletedat",
                table: "user");

            migrationBuilder.DropColumn(
                name: "deletedby",
                table: "user");

            migrationBuilder.DropColumn(
                name: "updatedat",
                table: "user");

            migrationBuilder.DropColumn(
                name: "updatedby",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "secretkey",
                newName: "SecretKey");

            migrationBuilder.RenameTable(
                name: "secret",
                newName: "Secret");

            migrationBuilder.RenameColumn(
                name: "passwordhash",
                table: "User",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "User",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "masterpasswordsalt",
                table: "User",
                newName: "MasterPasswordSalt");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "SecretKey",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "key",
                table: "SecretKey",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SecretKey",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_secretkey_userid",
                table: "SecretKey",
                newName: "IX_SecretKey_UserId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Secret",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "Secret",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Secret",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Secret",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Secret",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_secret_userid",
                table: "Secret",
                newName: "IX_Secret_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecretKey",
                table: "SecretKey",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Secret",
                table: "Secret",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Secret_User_UserId",
                table: "Secret",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecretKey_User_UserId",
                table: "SecretKey",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
