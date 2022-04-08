using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class PlotTBLUserIdPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlotTBL",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");


            migrationBuilder.AddForeignKey(
                name: "FK_PlotTBL_AspNetUsers_UserId",
                table: "PlotTBL",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTBL_AspNetUsers_UserId",
                table: "PlotTBL");


            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlotTBL");
        }
    }
}
