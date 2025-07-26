using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMmembers_Alumnis_MembersId",
                table: "CommunityMmembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMmembers_Communities_CommunitiesId",
                table: "CommunityMmembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityMmembers",
                table: "CommunityMmembers");

            migrationBuilder.RenameTable(
                name: "CommunityMmembers",
                newName: "CommunityMembers");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMmembers_MembersId",
                table: "CommunityMembers",
                newName: "IX_CommunityMembers_MembersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityMembers",
                table: "CommunityMembers",
                columns: new[] { "CommunitiesId", "MembersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Alumnis_MembersId",
                table: "CommunityMembers",
                column: "MembersId",
                principalTable: "Alumnis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Communities_CommunitiesId",
                table: "CommunityMembers",
                column: "CommunitiesId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Alumnis_MembersId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Communities_CommunitiesId",
                table: "CommunityMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityMembers",
                table: "CommunityMembers");

            migrationBuilder.RenameTable(
                name: "CommunityMembers",
                newName: "CommunityMmembers");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembers_MembersId",
                table: "CommunityMmembers",
                newName: "IX_CommunityMmembers_MembersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityMmembers",
                table: "CommunityMmembers",
                columns: new[] { "CommunitiesId", "MembersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMmembers_Alumnis_MembersId",
                table: "CommunityMmembers",
                column: "MembersId",
                principalTable: "Alumnis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMmembers_Communities_CommunitiesId",
                table: "CommunityMmembers",
                column: "CommunitiesId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
