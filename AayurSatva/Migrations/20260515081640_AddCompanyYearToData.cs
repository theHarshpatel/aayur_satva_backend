using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AayurSatva.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyYearToData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoId",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearId",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoId",
                table: "Billings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearId",
                table: "Billings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "CoId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "CoId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Appointments");
        }
    }
}
