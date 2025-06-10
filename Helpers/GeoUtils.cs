using Taxi_Booking.Models.Entities;

namespace Taxi_Booking.Helpers
{
    public static class GeoUtils
    {
        public static double GetDistanceInKm(DriverLocation loc1, Location loc2)
        {
            var R = 6371; // Radius of Earth in km
            var dLat = DegreesToRadians(loc2.Latitude - loc1.Latitude);
            var dLon = DegreesToRadians(loc2.Longitude - loc1.Longitude);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(loc1.Latitude)) * Math.Cos(DegreesToRadians(loc2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double deg) => deg * (Math.PI / 180);
    }

}
