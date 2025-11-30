using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatisWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialTripUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SelectedSeats",
                table: "SeatSelections",
                newName: "SelectedSeatsJson");

            migrationBuilder.AlterColumn<string>(
                name: "SeatLayout",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SaleDate",
                table: "Trips",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "BookedSeats",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSelections_TripId",
                table: "SeatSelections",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatSelections_Trips_TripId",
                table: "SeatSelections",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatSelections_Trips_TripId",
                table: "SeatSelections");

            migrationBuilder.DropIndex(
                name: "IX_SeatSelections_TripId",
                table: "SeatSelections");

            migrationBuilder.DropColumn(
                name: "BookedSeats",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "SelectedSeatsJson",
                table: "SeatSelections",
                newName: "SelectedSeats");

            migrationBuilder.AlterColumn<string>(
                name: "SeatLayout",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SaleDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
