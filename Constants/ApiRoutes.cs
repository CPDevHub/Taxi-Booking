using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Metadata;

namespace Taxi_Booking.Constants
{
    public class ApiRoutes
    {
        public const string Base = "api/v1/";
        public static class Driver
        {
            public const string SubmitRating = Base + "driver/rating/{driverId}";
            public const string Settings = Base + "driver/settings";
            public const string Dashboard = Base + "driver/dashboard";
            public const string History = Base + "driver/history";
            public const string location = Base + "driver/location/{driverId}";
        }
        public static class Passenger
        {
            public const string History = Base + "passenger/history";
        }
        public static class Authentication
        {
            public const string PassengerLogin = Base + "auth/passenger-login";
            public const string PassengerSignup = Base + "auth/passenger-signup";
            public const string DriverLogin = Base + "auth/driver-login";
            public const string DriverSignup = Base + "auth/driver-signup";
            public const string IsAuthenticated = Base + "auth/is-authenticated";
        }
        public static class Ride
        {
            public const string BookRide = Base + "ride/book";
        }
    }
}
