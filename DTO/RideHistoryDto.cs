namespace Taxi_Booking.DTO
{
    public class RideHistoryDto
    {
        public int Id { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double TotalFare { get; set; }
        public double CancellationCharges { get; set; }
        public DateTime RideStartAt { get; set; }
        public string Status { get; set; }
    }
}
