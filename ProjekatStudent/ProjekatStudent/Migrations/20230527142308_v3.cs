using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatStudent.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Exams_ExamId",
                table: "Joins");

            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Student_StudentId",
                table: "Joins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Joins",
                table: "Joins");

            migrationBuilder.DropIndex(
                name: "IX_Joins_StudentId",
                table: "Joins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Joins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Joins",
                table: "Joins",
                columns: new[] { "StudentId", "ExamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Exams_ExamId",
                table: "Joins",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Student_StudentId",
                table: "Joins",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Exams_ExamId",
                table: "Joins");

            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Student_StudentId",
                table: "Joins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Joins",
                table: "Joins");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Joins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Joins",
                table: "Joins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Joins_StudentId",
                table: "Joins",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Exams_ExamId",
                table: "Joins",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Student_StudentId",
                table: "Joins",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
