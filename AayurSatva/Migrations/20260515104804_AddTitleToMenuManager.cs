using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AayurSatva.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToMenuManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MenuManagers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "MenuManagers");
        }
    }
}
