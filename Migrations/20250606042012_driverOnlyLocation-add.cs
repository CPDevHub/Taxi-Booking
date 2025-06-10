using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class driverOnlyLocationadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude_Latitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Latitude_Longitude",
                table: "Driver");

            migrationBuilder.RenameColumn(
                name: "Longitude_Longitude",
                table: "Driver",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Longitude_Latitude",
                table: "Driver",
                newName: "Location_Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Driver",
                newName: "Longitude_Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Driver",
                newName: "Longitude_Latitude");

            migrationBuilder.AddColumn<double>(
                name: "Latitude_Latitude",
                table: "Driver",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude_Longitude",
                table: "Driver",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
