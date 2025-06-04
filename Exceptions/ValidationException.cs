namespace Taxi_Booking.Exceptions
{
    public class ValidationException:Exception
    {
        public List<string> Errors { get; }
        public ValidationException(string message,List<string> errors) : base(message) {
            Errors = errors ?? new List<string>();
        }
    }
}
