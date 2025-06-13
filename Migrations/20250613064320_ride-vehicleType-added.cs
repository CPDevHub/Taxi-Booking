using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class ridevehicleTypeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RideVehicle",
                table: "Ride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RideVehicle",
                table: "Ride");
        }
    }
}
