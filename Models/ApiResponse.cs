using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Taxi_Booking.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }


        //null work only for refrence and default works for both reference types(null) and value types(0, false, etc.).
        public ApiResponse(string message = "", bool success = true, T ?data=default)
        {
            Success = success;
            Message = message;
            Data = data;
        }


    }
}
