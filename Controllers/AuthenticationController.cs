using Microsoft.AspNetCore.Mvc;
using Taxi_Booking.DTO.Authentication;
using Taxi_Booking.Helpers;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Passengers;
using Taxi_Booking.Services.Token;
using Taxi_Booking.Exceptions;
using Taxi_Booking.Models.Responses;

using Taxi_Booking.DTO.Responses;
using AutoMapper;
using System;
using Taxi_Booking.Models.Enums;


namespace Taxi_Booking.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IDriverService _driverService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly PasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthenticationController(IPassengerService passengerService, IDriverService driverService, ILogger<AuthenticationController> logger, PasswordHasher passwordHasher, ITokenService tokenService, IMapper mapper)
        {
            _passengerService = passengerService;
            _driverService = driverService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("passenger-login")]
        public async Task<IActionResult> PassengerLogin([FromBody] LoginDto loginDetails)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.Values=>gives ModelStateEntry and Each ModelStateEntry represents one input field and contains a list of errors.
                //SelectMany flattens all those individual lists of errors into a single collection of all errors across all inputs.
                //each error(e) is a ModelError object.This selects just the ErrorMessage property from each error.
                //Converts the result into a list of strings
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Validation Failed", errorMessages);
            }

            var passenger =await _passengerService.GetPassengerByEmailAsync(loginDetails.Email);
            if (passenger == null) throw new NotFoundException("User does not exist with this Email");
            if (!_passwordHasher.Verify(loginDetails.Password, passenger.PasswordHash)) throw new InvalidCredentialException("Please provide a correct password");
            string token = _tokenService.CreateToken(passenger.Id,"Passenger");

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true, // Set to true in production with HTTPS
                //SameSite = SameSiteMode.Strict, // Or Lax depending on your needs
                Expires = DateTime.UtcNow.AddHours(3)
            });

            return Ok(new ApiResponse<PassengerResponseDto>("Login successful", true, _mapper.Map<PassengerResponseDto>(passenger)));
        }
        [HttpPost("passenger-signup")]
        public async Task<IActionResult> PassengerSignup([FromBody] PassengerRegisterDto signupDetails)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Validation Failed",errorMessages);
            }
            await _passengerService.CreatePassengerAsync(signupDetails);
            return Ok(new ApiResponse<string>("Signup Successful", true));
        }

        [HttpPost("driver-login")]
        public async Task<IActionResult> DriverLogin([FromBody] LoginDto loginDetails)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Validation Failed", errorMessages);
            }

            var driver = await _driverService.GetDriverByEmailAsync(loginDetails.Email);
            if (driver == null) throw new NotFoundException("User does not exist with this Email");

            if (!_passwordHasher.Verify(loginDetails.Password, driver.PasswordHash)) throw new InvalidCredentialException("Invalid Email or Password");

            await _driverService.UpdateDriverStatus(DriverStatus.Available, driver.Id);

            string token = _tokenService.CreateToken(driver.Id, "Driver");
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true, // Set to true in production with HTTPS
                //SameSite = SameSiteMode.Strict, // Or Lax depending on your needs
                Expires = DateTime.UtcNow.AddHours(3)
            });

            return Ok(new ApiResponse<DriverResponseDto>("Login successful", true, _mapper.Map<DriverResponseDto>(driver)));

        }

        [HttpPost("driver-signup")]
        public async Task<IActionResult> DriverSignup([FromBody] DriverRegisterDto signupDetails)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Signup Details: {@SignupDetails}", signupDetails);
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Validation Failed", errorMessages);
            }
            await _driverService.CreateDriverAsync(signupDetails);
            return Ok(new ApiResponse<string>("Driver Signup Successful", true));
        }

    }
}
