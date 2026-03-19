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
            migrationBuilder.Sql(
                """
                ALTER TABLE "user" DROP COLUMN IF EXISTS master_password_salt;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE "secret_key"
                ADD COLUMN IF NOT EXISTS key_salt bytea NOT NULL DEFAULT '\\x'::bytea;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "secret_key" DROP COLUMN IF EXISTS key_salt;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE "user"
                ADD COLUMN IF NOT EXISTS master_password_salt bytea NOT NULL DEFAULT '\\x'::bytea;
                """);
        }
    }
}
