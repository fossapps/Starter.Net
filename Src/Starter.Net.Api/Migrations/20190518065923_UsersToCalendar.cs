using Microsoft.EntityFrameworkCore.Migrations;

namespace Starter.Net.Api.Migrations
{
    public partial class UsersToCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserToCalendars",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Calendar = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToCalendars", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "12de2df2-a0c5-402f-ac8f-d24914c01b38");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToCalendars");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6eb015-82d1-480c-b962-5aab596ef4f6",
                column: "ConcurrencyStamp",
                value: "aea2e5a7-7336-4fe2-b6e9-5b8581071ffa");
        }
    }
}
