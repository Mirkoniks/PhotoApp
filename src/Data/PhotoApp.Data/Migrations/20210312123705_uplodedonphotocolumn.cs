using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoApp.Data.Migrations
{
    public partial class uplodedonphotocolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedOn",
                table: "Photos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedOn",
                table: "Photos");
        }
    }
}
