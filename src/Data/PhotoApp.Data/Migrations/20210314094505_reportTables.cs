using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoApp.Data.Migrations
{
    public partial class reportTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportedSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubjectId = table.Column<int>(nullable: false),
                    IsPhoto = table.Column<bool>(nullable: false),
                    IsUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Descripton = table.Column<string>(nullable: true),
                    ReportedSubjectId = table.Column<int>(nullable: false),
                    ReportedOn = table.Column<DateTime>(nullable: false),
                    IsResolved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportReportedSubjects",
                columns: table => new
                {
                    ReportId = table.Column<string>(nullable: false),
                    ReportedSubjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportReportedSubjects", x => new { x.ReportId, x.ReportedSubjectId });
                    table.ForeignKey(
                        name: "FK_ReportReportedSubjects_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportReportedSubjects_ReportedSubjects_ReportedSubjectId",
                        column: x => x.ReportedSubjectId,
                        principalTable: "ReportedSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportReportedSubjects_ReportedSubjectId",
                table: "ReportReportedSubjects",
                column: "ReportedSubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportReportedSubjects");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ReportedSubjects");
        }
    }
}
