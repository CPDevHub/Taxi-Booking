using System.ComponentModel.DataAnnotations;

namespace Taxi_Booking.DTO.Authentication
{
    public class PassengerRegisterDto
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
        public string? AvatarUrl { get; set; }
    }
}
