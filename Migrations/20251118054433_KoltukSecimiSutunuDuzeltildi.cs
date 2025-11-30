using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatisWebApp.Migrations
{
    /// <inheritdoc />
    public partial class KoltukSecimiSutunuDuzeltildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
          name: "SelectedSeats",
          table: "SeatSelections",
          type: "nvarchar(max)",
          nullable: true, // Eğer modelinizde List<string> nullable ise true yapın
          oldClrType: typeof(int) // Veritabanındaki eski tipini belirtin (INT ise INT)
          );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>( // Buraya eski tipi yazın
                 name: "SelectedSeats",
                 table: "SeatSelections",
                 type: "int", // Buraya eski tipi yazın
                 nullable: false, // Buraya eski nullable durumunu yazın
                 oldClrType: typeof(string),
                 oldNullable: true);
        }
    }
}
