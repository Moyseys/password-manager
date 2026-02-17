using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class DashboardVaultifyView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""dashboard_vaultify"" AS
                SELECT  
                    u.""id"" AS ""user_id"",
                    COUNT(s.id) as ""total_secrets""
                FROM ""secret"" s
                LEFT JOIN ""user"" u ON u.""id"" = s.""user_id""
                GROUP BY u.""id"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS ""dashboard_vaultify"";
            ");
        }
    }
}
