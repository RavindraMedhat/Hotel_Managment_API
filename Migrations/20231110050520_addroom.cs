using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel_Managment_API.Migrations
{
    public partial class addroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomCategoryTB",
                table: "RoomCategoryTB");

            migrationBuilder.RenameTable(
                name: "RoomCategoryTB",
                newName: "roomCategoryTB");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "roomCategoryTB",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomCategoryTB",
                table: "roomCategoryTB",
                column: "Category_ID");

            migrationBuilder.CreateTable(
                name: "RoomTB",
                columns: table => new
                {
                    Room_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_ID = table.Column<int>(nullable: false),
                    Branch_ID = table.Column<int>(nullable: false),
                    Hotel_ID = table.Column<int>(nullable: false),
                    Room_No = table.Column<string>(maxLength: 7, nullable: false),
                    Room_Description = table.Column<string>(maxLength: 300, nullable: false),
                    Room_Price = table.Column<float>(nullable: false),
                    Iminity_Pool = table.Column<bool>(nullable: false),
                    Iminity_Bath = table.Column<bool>(nullable: false),
                    Iminity_NoOfBed = table.Column<int>(nullable: false),
                    Active_Flag = table.Column<bool>(nullable: false),
                    Delete_Flag = table.Column<bool>(nullable: false),
                    sortedfield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTB", x => x.Room_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomTB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomCategoryTB",
                table: "roomCategoryTB");

            migrationBuilder.RenameTable(
                name: "roomCategoryTB",
                newName: "RoomCategoryTB");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoomCategoryTB",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomCategoryTB",
                table: "RoomCategoryTB",
                column: "Category_ID");
        }
    }
}
