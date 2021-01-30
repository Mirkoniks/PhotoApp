using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoApp.Data.Migrations
{
    public partial class LikingSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpcoming",
                table: "Challanges",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UsersPhotoLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PhotoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPhotoLikes", x => new { x.PhotoId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersPhotoLikes_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersPhotoLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersPhotoLikes_UserId",
                table: "UsersPhotoLikes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersPhotoLikes");

            migrationBuilder.DropColumn(
                name: "IsUpcoming",
                table: "Challanges");
        }
    }
}
