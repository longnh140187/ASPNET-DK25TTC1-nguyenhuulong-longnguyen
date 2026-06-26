using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeWebsite.Web.Migrations;

[DbContext(typeof(Data.AppDbContext))]
[Migration("20260625100000_AddBlogsTable")]
public partial class AddBlogsTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "blogs",
            columns: table => new
            {
                id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                slug = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                thumbnail_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                summary = table.Column<string>(type: "text", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                content = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                status = table.Column<string>(type: "enum('DRAFT','PUBLISHED')", nullable: false, defaultValue: "DRAFT")
                    .Annotation("MySql:CharSet", "utf8mb4"),
                created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false),
                updated_at = table.Column<DateTime>(type: "datetime(3)", nullable: false),
                deleted_at = table.Column<DateTime>(type: "datetime(3)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_blogs", x => x.id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_blogs_slug",
            table: "blogs",
            column: "slug",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "blogs");
    }
}
