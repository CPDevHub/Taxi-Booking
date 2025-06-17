using System.ComponentModel.DataAnnotations;

namespace Taxi_Booking.Models
{
    public class Location
    {
        [Required(ErrorMessage = "Latitude is Required")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is Required")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
    }

}
