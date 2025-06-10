using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class driverLocationadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude_Address",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Longitude_Address",
                table: "Driver");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude_Address",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Longitude_Address",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
