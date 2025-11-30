using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatisWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatLayoutToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeatLayout",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatLayout",
                table: "Trips");
        }
    }
}
