using System.ComponentModel.DataAnnotations;
using Taxi_Booking.Models;

namespace Taxi_Booking.DTO.Authentication
{
    public class DriverRegisterDto
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Contact Number is Required")]
        [Phone(ErrorMessage = "Invalid Phone number format")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Avatar Image is Required")]
        public string AvatarUrl { get; set; }

        [Required(ErrorMessage = "Vehicle Type is Required")]
        public VehicleType DriverVehicleType { get; set; }

        [Required(ErrorMessage = "Vehicle Number is Required")]
        public string VehicleNumber { get; set; }

        [Required(ErrorMessage = "Vehicle Model is Required")]
        public string VehicleModel { get; set; }
    }
}
