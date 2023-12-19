using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class groupidinttostringinBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Group_ID",
                table: "Billing",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Group_ID",
                table: "Billing",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
