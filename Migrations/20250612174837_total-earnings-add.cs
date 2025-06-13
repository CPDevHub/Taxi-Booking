using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxi_Booking.Migrations
{
    /// <inheritdoc />
    public partial class totalearningsadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalEarnings",
                table: "Driver",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEarnings",
                table: "Driver");
        }
    }
}
