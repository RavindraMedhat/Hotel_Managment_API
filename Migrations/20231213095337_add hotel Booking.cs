using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class addhotelBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Booking_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(nullable: false),
                    Room_ID = table.Column<int>(nullable: false),
                    Group_ID = table.Column<int>(nullable: false),
                    Branch_ID = table.Column<int>(nullable: false),
                    Check_In_Time = table.Column<DateTime>(nullable: false),
                    Check_Out_Time = table.Column<DateTime>(nullable: false),
                    Booking_Date = table.Column<DateTime>(nullable: false),
                    Payment_Status = table.Column<string>(nullable: false),
                    Payed_Amount = table.Column<float>(nullable: false),
                    Payment_Mode = table.Column<string>(nullable: false),
                    Coupon_Code = table.Column<string>(nullable: true),
                    Booking_Status = table.Column<string>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    Customer_status = table.Column<string>(nullable: false),
                    Active_Flag = table.Column<bool>(nullable: false),
                    Delete_Flag = table.Column<bool>(nullable: false),
                    Sortedfield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Booking_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");
        }
    }
}
