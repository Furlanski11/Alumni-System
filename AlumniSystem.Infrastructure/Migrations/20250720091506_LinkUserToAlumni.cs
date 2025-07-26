using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToAlumni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "IdentityUser");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Alumnis",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumnis_UserId",
                table: "Alumnis",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnis_AspNetUsers_UserId",
                table: "Alumnis",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnis_AspNetUsers_UserId",
                table: "Alumnis");

            migrationBuilder.DropIndex(
                name: "IX_Alumnis_UserId",
                table: "Alumnis");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Alumnis");
        }
    }
}
