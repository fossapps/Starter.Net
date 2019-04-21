using Microsoft.EntityFrameworkCore.Migrations;

namespace Starter.Net.Api.Migrations
{
    public partial class Invitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FromUserId = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    NormalizedTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "5efd96b7-15a4-44a4-adab-c3c2b682efc6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "80ae3792-b254-4f47-89cf-f9308e0a4dd9");
        }
    }
}
