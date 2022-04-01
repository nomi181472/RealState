using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class SocietyTBLAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocietyTBL",
                columns: table => new
                {
                    SocietyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocietyTBL", x => x.SocietyId);
                    table.ForeignKey(
                        name: "FK_SocietyTBL_LocationTBL_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationTBL",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocietyTBL_LocationId",
                table: "SocietyTBL",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocietyTBL");
        }
    }
}
