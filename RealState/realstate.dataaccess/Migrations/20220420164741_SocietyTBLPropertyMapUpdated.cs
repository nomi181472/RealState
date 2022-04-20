using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class SocietyTBLPropertyMapUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "map",
                table: "SocietyTBL",
                newName: "Map");

            migrationBuilder.AlterColumn<string>(
                name: "Map",
                table: "SocietyTBL",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Map",
                table: "SocietyTBL",
                newName: "map");

            migrationBuilder.AlterColumn<string>(
                name: "map",
                table: "SocietyTBL",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
