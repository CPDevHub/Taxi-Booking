using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class driverlocationadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude_Address",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "Longitude_Address",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Longitude_Latitude",
                table: "Driver",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude_Longitude",
                table: "Driver",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude_Address",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Latitude_Latitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Latitude_Longitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Longitude_Address",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Longitude_Latitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Longitude_Longitude",
                table: "Driver");
        }
    }
}
