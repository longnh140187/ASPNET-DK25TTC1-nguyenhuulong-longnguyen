using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Web.Migrations;

[DbContext(typeof(Data.AppDbContext))]
[Migration("20260625090000_SeedAdminUser")]
public partial class SeedAdminUser : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
INSERT INTO users (id, name, username, password_hash, role, status, created_at, updated_at, deleted_at)
SELECT
    'd8a0f74f-cf52-4fcd-97bb-d0f3a644f7e1',
    'Administrator',
    'admin',
    '0336b1504c54b042bd75e65e96ca555d51d7a949251752577a35aea47c1705ee',
    2,
    1,
    UTC_TIMESTAMP(3),
    UTC_TIMESTAMP(3),
    NULL
WHERE NOT EXISTS (
    SELECT 1 FROM users WHERE username = 'admin'
);
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM users WHERE id = 'd8a0f74f-cf52-4fcd-97bb-d0f3a644f7e1';");
    }
}
