namespace Taxi_Booking.Models
{


    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string? StackTrace { get; set; }
        public List<string>? Errors { get; set; }

        public ErrorResponse(string message, int statusCode, string? stackTrace = null, List<string>? errors = null)
        {
            Message = message;
            StatusCode = statusCode;
            StackTrace = stackTrace;
            Errors = errors;
        }
    }
}
