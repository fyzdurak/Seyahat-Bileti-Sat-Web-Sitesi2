using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatisWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Trip_BookedSeatsJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookedSeats",
                table: "Trips",
                newName: "BookedSeatsJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookedSeatsJson",
                table: "Trips",
                newName: "BookedSeats");
        }
    }
}
