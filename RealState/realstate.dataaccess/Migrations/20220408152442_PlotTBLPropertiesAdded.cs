using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class PlotTBLPropertiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Area",
                table: "PlotTBL",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "PlotTBL",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompleteAddress",
                table: "PlotTBL",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PlotSize",
                table: "PlotTBL",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "PlotTBL");

            migrationBuilder.DropColumn(
                name: "CompleteAddress",
                table: "PlotTBL");

            migrationBuilder.DropColumn(
                name: "PlotSize",
                table: "PlotTBL");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PlotTBL",
                newName: "Area");
        }
    }
}
