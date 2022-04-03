using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class ApplicationUserTBLPropertiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerifiedUserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VerifiedUserId",
                table: "AspNetUsers",
                column: "VerifiedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_VerifiedUserTBL_VerifiedUserId",
                table: "AspNetUsers",
                column: "VerifiedUserId",
                principalTable: "VerifiedUserTBL",
                principalColumn: "VerifiedUserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_VerifiedUserTBL_VerifiedUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VerifiedUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VerifiedUserId",
                table: "AspNetUsers");
        }
    }
}
