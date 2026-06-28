#!/usr/bin/env python3
"""Export active DB rows to EF Core seed migration (REPLACE INTO)."""

import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
MIGRATION_PATH = ROOT / "src/Migrations/20260628120000_SeedFullDummyData.cs"
MIGRATION_ID = "20260628120000_SeedFullDummyData"


def dump_table(table: str) -> list[str]:
    result = subprocess.run(
        [
            "mysqldump",
            "-h", "127.0.0.1",
            "-P", "3306",
            "-u", "root",
            "-proot",
            "recipes",
            table,
            "--no-create-info",
            "--complete-insert",
            "--skip-extended-insert",
            "--compact",
            "--where=deleted_at IS NULL",
        ],
        capture_output=True,
        text=True,
        check=True,
    )
    lines = []
    for line in result.stdout.splitlines():
        line = line.strip()
        if not line.startswith("INSERT INTO"):
            continue
        lines.append(line.replace("INSERT INTO", "REPLACE INTO", 1))
    return lines


def to_csharp_verbatim(sql: str) -> str:
    return sql.replace('"', '""')


def build_migration(statements: list[str]) -> str:
    sql = "\n".join(statements)
    escaped = to_csharp_verbatim(sql)

    return f"""using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Web.Migrations;

[DbContext(typeof(Data.AppDbContext))]
[Migration("{MIGRATION_ID}")]
public partial class SeedFullDummyData : Migration
{{
    protected override void Up(MigrationBuilder migrationBuilder)
    {{
        migrationBuilder.Sql(@"
{escaped}
");
    }}

    protected override void Down(MigrationBuilder migrationBuilder)
    {{
        migrationBuilder.Sql(@"
DELETE FROM blogs;
DELETE FROM recipes;
DELETE FROM categories WHERE id NOT IN (
    'c1000001-0000-4000-8000-000000000001',
    'c1000002-0000-4000-8000-000000000002',
    'c1000003-0000-4000-8000-000000000003'
);
");
    }}
}}
"""


def main() -> None:
    statements: list[str] = []
    for table in ("categories", "users", "recipes", "blogs"):
        rows = dump_table(table)
        print(f"{table}: {len(rows)} rows")
        statements.extend(rows)

    MIGRATION_PATH.write_text(build_migration(statements), encoding="utf-8")
    print(f"Written: {MIGRATION_PATH.relative_to(ROOT)} ({MIGRATION_PATH.stat().st_size:,} bytes)")


if __name__ == "__main__":
    main()
