using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class baseentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_secret_user_userid",
                table: "secret");

            migrationBuilder.DropForeignKey(
                name: "FK_secretkey_user_userid",
                table: "secretkey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_secretkey",
                table: "secretkey");

            migrationBuilder.RenameTable(
                name: "secretkey",
                newName: "secret_key");

            migrationBuilder.RenameColumn(
                name: "updatedby",
                table: "user",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "updatedat",
                table: "user",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "passwordhash",
                table: "user",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "masterpasswordsalt",
                table: "user",
                newName: "master_password_salt");

            migrationBuilder.RenameColumn(
                name: "deletedby",
                table: "user",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "deletedat",
                table: "user",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "user",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "user",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "secret",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_secret_userid",
                table: "secret",
                newName: "IX_secret_user_id");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "secret_key",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_secretkey_userid",
                table: "secret_key",
                newName: "IX_secret_key_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_secret_key",
                table: "secret_key",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_secret_user_user_id",
                table: "secret",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_secret_key_user_user_id",
                table: "secret_key",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_secret_user_user_id",
                table: "secret");

            migrationBuilder.DropForeignKey(
                name: "FK_secret_key_user_user_id",
                table: "secret_key");

            migrationBuilder.DropPrimaryKey(
                name: "PK_secret_key",
                table: "secret_key");

            migrationBuilder.RenameTable(
                name: "secret_key",
                newName: "secretkey");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "user",
                newName: "updatedby");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "user",
                newName: "updatedat");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "user",
                newName: "passwordhash");

            migrationBuilder.RenameColumn(
                name: "master_password_salt",
                table: "user",
                newName: "masterpasswordsalt");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "user",
                newName: "deletedby");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "user",
                newName: "deletedat");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "user",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "secret",
                newName: "userid");

            migrationBuilder.RenameIndex(
                name: "IX_secret_user_id",
                table: "secret",
                newName: "IX_secret_userid");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "secretkey",
                newName: "userid");

            migrationBuilder.RenameIndex(
                name: "IX_secret_key_user_id",
                table: "secretkey",
                newName: "IX_secretkey_userid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_secretkey",
                table: "secretkey",
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
    }
}
