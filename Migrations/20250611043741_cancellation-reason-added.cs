using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class cancellationreasonadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Ride",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Ride");
        }
    }
}
