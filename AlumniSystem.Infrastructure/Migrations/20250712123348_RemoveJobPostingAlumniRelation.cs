using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJobPostingAlumniRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Alumnis_AlumniId",
                table: "JobPostings");

            migrationBuilder.AlterColumn<int>(
                name: "AlumniId",
                table: "JobPostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Alumnis_AlumniId",
                table: "JobPostings",
                column: "AlumniId",
                principalTable: "Alumnis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Alumnis_AlumniId",
                table: "JobPostings");

            migrationBuilder.AlterColumn<int>(
                name: "AlumniId",
                table: "JobPostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Alumnis_AlumniId",
                table: "JobPostings",
                column: "AlumniId",
                principalTable: "Alumnis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
