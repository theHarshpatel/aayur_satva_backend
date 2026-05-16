using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AayurSatva.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubMenuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubMenuIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalAccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuManagers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuManagers");
        }
    }
}
