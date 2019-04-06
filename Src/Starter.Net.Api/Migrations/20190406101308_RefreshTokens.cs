using Microsoft.EntityFrameworkCore.Migrations;

namespace Starter.Net.Api.Migrations
{
    public partial class RefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "1ce4a46c-55ce-4bb0-8c22-79e3d8253b63");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "b0747176-df24-4a1c-b5a5-8653a4459d09");
        }
    }
}
