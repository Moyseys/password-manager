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
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret' AND column_name = 'password'
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret' AND column_name = 'iv'
                    ) THEN
                        ALTER TABLE "secret" RENAME COLUMN password TO iv;
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE "secret"
                ADD COLUMN IF NOT EXISTS cipher_password bytea NOT NULL DEFAULT '\\x'::bytea;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE "secret"
                ADD COLUMN IF NOT EXISTS website text NOT NULL DEFAULT '';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "secret" DROP COLUMN IF EXISTS cipher_password;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE "secret" DROP COLUMN IF EXISTS website;
                """);

            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret' AND column_name = 'iv'
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret' AND column_name = 'password'
                    ) THEN
                        ALTER TABLE "secret" RENAME COLUMN iv TO password;
                    END IF;
                END $$;
                """);
        }
    }
}
