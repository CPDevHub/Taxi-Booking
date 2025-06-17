namespace Taxi_Booking.Constants
{
    public class SignalREvents
    {
        public const string SendLocation = "SendLocation";
        public const string RideStart = "RideStart";
        public const string RideAccepted = "RideAccepted";
        public const string RideAcceptedUserNotify = "RideAcceptedUserNotify";
        public const string RideAlreadyAccepted = "RideAlreadyAccepted";
        public const string RideAcceptedDriverNotify = "RideAcceptedDriverNotify";
        public const string RideCancelledByPassengerBeforeAccept = "RideCancelledByPassengerBeforeAccept";
        public const string RideCancelledByDriver = "RideCancelledByDriver";
        public const string RideCancelledByPassenger = "RideCancelledByPassenger";
        public const string RideCompleted = "RideCompleted";
    }
}
