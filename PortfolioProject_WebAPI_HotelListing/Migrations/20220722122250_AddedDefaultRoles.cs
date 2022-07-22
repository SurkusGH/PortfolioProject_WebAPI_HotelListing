using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioProject_WebAPI_HotelListing.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "076438b1-f98e-4452-a330-14cd3ad7069b", "aedf92f8-b000-4641-93d5-4491574f5adb", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5ee1bb82-73cd-4a81-978d-4b187ce570a2", "49285e1d-ac54-415c-ab29-b79b3da1e775", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "076438b1-f98e-4452-a330-14cd3ad7069b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ee1bb82-73cd-4a81-978d-4b187ce570a2");
        }
    }
}
