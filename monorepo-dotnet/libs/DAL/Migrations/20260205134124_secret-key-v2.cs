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
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret_key' AND column_name = 'key_salt'
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret_key' AND column_name = 'salt'
                    ) THEN
                        ALTER TABLE "secret_key" RENAME COLUMN key_salt TO salt;
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS algorithm text NOT NULL DEFAULT ''; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS derivation_algorithm text NOT NULL DEFAULT ''; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS hash_algorithm text NOT NULL DEFAULT ''; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS iterations integer NOT NULL DEFAULT 0; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS key_iv bytea NOT NULL DEFAULT '\\x'::bytea; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS key_size integer NOT NULL DEFAULT 0; ");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" ADD COLUMN IF NOT EXISTS salt_size integer NOT NULL DEFAULT 0; ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS algorithm;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS derivation_algorithm;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS hash_algorithm;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS iterations;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS key_iv;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS key_size;");
            migrationBuilder.Sql("ALTER TABLE \"secret_key\" DROP COLUMN IF EXISTS salt_size;");

            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret_key' AND column_name = 'salt'
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = 'public' AND table_name = 'secret_key' AND column_name = 'key_salt'
                    ) THEN
                        ALTER TABLE "secret_key" RENAME COLUMN salt TO key_salt;
                    END IF;
                END $$;
                """);
        }
    }
}
