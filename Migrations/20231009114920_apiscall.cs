using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class apiscall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hotelBranchTBs",
                columns: table => new
                {
                    Branch_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hotel_ID = table.Column<int>(nullable: false),
                    Branch_Name = table.Column<string>(maxLength: 50, nullable: false),
                    Branch_Description = table.Column<string>(maxLength: 300, nullable: false),
                    Branch_map_coordinate = table.Column<string>(nullable: false),
                    Branch_Address = table.Column<string>(maxLength: 50, nullable: false),
                    Branch_Contact_No = table.Column<string>(nullable: false),
                    Branch_Email_Adderss = table.Column<string>(nullable: false),
                    Branch_Contect_Person = table.Column<string>(maxLength: 50, nullable: false),
                    Active_Flag = table.Column<bool>(nullable: false),
                    Delete_Flag = table.Column<bool>(nullable: false),
                    sortedfield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotelBranchTBs", x => x.Branch_ID);
                });

            migrationBuilder.CreateTable(
                name: "hotels",
                columns: table => new
                {
                    Hotel_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hotel_Name = table.Column<string>(maxLength: 50, nullable: false),
                    Hotel_Description = table.Column<string>(maxLength: 300, nullable: false),
                    Hotel_map_coordinate = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    Contact_No = table.Column<string>(nullable: false),
                    Email_Adderss = table.Column<string>(nullable: false),
                    Contect_Person = table.Column<string>(maxLength: 50, nullable: false),
                    Standard_check_In_Time = table.Column<string>(nullable: false),
                    Standard_check_out_Time = table.Column<string>(nullable: false),
                    Active_Flag = table.Column<bool>(nullable: false),
                    Delete_Flag = table.Column<bool>(nullable: false),
                    sortedfield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotels", x => x.Hotel_ID);
                });

            migrationBuilder.CreateTable(
                name: "imageMasterTBs",
                columns: table => new
                {
                    Image_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image_URl = table.Column<string>(nullable: false),
                    Reference_ID = table.Column<int>(nullable: false),
                    ReferenceTB_Name = table.Column<string>(nullable: false),
                    Active_Flag = table.Column<bool>(nullable: false),
                    Delete_Flag = table.Column<bool>(nullable: false),
                    sortedfield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imageMasterTBs", x => x.Image_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hotelBranchTBs");

            migrationBuilder.DropTable(
                name: "hotels");

            migrationBuilder.DropTable(
                name: "imageMasterTBs");
        }
    }
}
