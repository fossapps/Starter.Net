using Microsoft.EntityFrameworkCore.Migrations;

namespace Starter.Net.Api.Migrations
{
    public partial class UpdateRefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "RefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Useragent",
                table: "RefreshTokens",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "80ae3792-b254-4f47-89cf-f9308e0a4dd9");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Useragent",
                table: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "1ce4a46c-55ce-4bb0-8c22-79e3d8253b63");
        }
    }
}
