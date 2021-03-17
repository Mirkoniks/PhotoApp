using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoApp.Data.Migrations
{
    public partial class challangeIdInUserPhotsLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChallangeId",
                table: "UsersPhotoLikes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallangeId",
                table: "UsersPhotoLikes");
        }
    }
}
