using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoApp.Data.Migrations
{
    public partial class reportTablesRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportReportedSubjects");

            migrationBuilder.DropTable(
                name: "ReportedSubjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportedSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsPhoto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportReportedSubjects",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ReportedSubjectId = table.Column<int>(type: "int", nullable: false)
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
    }
}
