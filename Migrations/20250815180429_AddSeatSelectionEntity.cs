using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatisWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatSelectionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeatSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    SelectedSeats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatSelections", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeatSelections");
        }
    }
}
