using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class login2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseUser_ID",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "User_ID",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "UseUser_ID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
