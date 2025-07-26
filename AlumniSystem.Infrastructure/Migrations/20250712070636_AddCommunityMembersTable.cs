using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityMembersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumniCommunity_Alumnis_MembersId",
                table: "AlumniCommunity");

            migrationBuilder.DropForeignKey(
                name: "FK_AlumniCommunity_Communities_CommunitiesId",
                table: "AlumniCommunity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlumniCommunity",
                table: "AlumniCommunity");

            migrationBuilder.RenameTable(
                name: "AlumniCommunity",
                newName: "CommunityMmembers");

            migrationBuilder.RenameIndex(
                name: "IX_AlumniCommunity_MembersId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "AlumniCommunity");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMmembers_MembersId",
                table: "AlumniCommunity",
                newName: "IX_AlumniCommunity_MembersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlumniCommunity",
                table: "AlumniCommunity",
                columns: new[] { "CommunitiesId", "MembersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniCommunity_Alumnis_MembersId",
                table: "AlumniCommunity",
                column: "MembersId",
                principalTable: "Alumnis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumniCommunity_Communities_CommunitiesId",
                table: "AlumniCommunity",
                column: "CommunitiesId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
