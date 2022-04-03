using Microsoft.EntityFrameworkCore.Migrations;

namespace realstate.dataaccess.Migrations
{
    public partial class VerifiedUserTBLADDED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerifiedUserTBL",
                columns: table => new
                {
                    VerifiedUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    SecurityLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedUserTBL", x => x.VerifiedUserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifiedUserTBL");
        }
    }
}
