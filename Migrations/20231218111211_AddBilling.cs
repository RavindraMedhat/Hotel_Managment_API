using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class AddBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Payed_Amount",
                table: "Billing",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payed_Amount",
                table: "Billing");
        }
    }
}
