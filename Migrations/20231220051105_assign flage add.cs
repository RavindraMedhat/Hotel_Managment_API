using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class assignflageadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Assign_Flag",
                table: "Coupon",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Assign_UId",
                table: "Coupon",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assign_Flag",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "Assign_UId",
                table: "Coupon");
        }
    }
}
