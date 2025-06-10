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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Taxi_Booking.Hubs;
using Taxi_Booking.Models.Entities;


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
        private readonly IHubContext<TaxiBookingHub> _taxiHub;
        public AuthenticationController(IPassengerService passengerService, IDriverService driverService, ILogger<AuthenticationController> logger, PasswordHasher passwordHasher, ITokenService tokenService, IMapper mapper, IHubContext<TaxiBookingHub> taxiHub)
        {
            _passengerService = passengerService;
            _driverService = driverService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _taxiHub = taxiHub;
            _mapper = mapper;
        }
        [HttpPost("passenger-login")]
        public async Task<IActionResult> PassengerLogin([FromBody] LoginDto loginDetails)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDetails.Email);
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                _logger.LogWarning("Login validation failed for email: {Email}. Errors: {Errors}",
           loginDetails.Email, string.Join(", ", errorMessages));
                throw new ValidationException("Validation Failed", errorMessages);
            }

            var passenger =await _passengerService.GetPassengerByEmailAsync(loginDetails.Email);
            if (passenger == null)
            {
                _logger.LogWarning("Login failed: No user found with email: {Email}", loginDetails.Email);
                throw new NotFoundException("User does not exist with this Email");
            }
            if (!_passwordHasher.Verify(loginDetails.Password, passenger.PasswordHash))
            {
                _logger.LogWarning("Login failed: Incorrect password for email: {Email}", loginDetails.Email);
                throw new InvalidCredentialException("Please provide a correct password");
            }
            string token = _tokenService.CreateToken(passenger.Id,"Passenger");


            _logger.LogInformation("Login successful for email: {Email}", loginDetails.Email);

            var responseDto = _mapper.Map<PassengerResponseDto>(passenger);
            var loginResult = new
            {
                Token = token,
                response = responseDto
            };

            return Ok(new ApiResponse<object>("Login successful", true,loginResult));
        }
        [HttpPost("passenger-signup")]
        public async Task<IActionResult> PassengerSignup([FromBody] PassengerRegisterDto signupDetails)
        {
            _logger.LogInformation("Received signup request for passenger with email: {Email}", signupDetails.Email);
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Passenger signup validation failed for email: {Email}. Errors: {Errors}", signupDetails.Email, string.Join(", ", errorMessages));
                throw new ValidationException("Validation Failed",errorMessages);
            }
            await _passengerService.CreatePassengerAsync(signupDetails);
            _logger.LogInformation("Passenger signup successful for email: {Email}", signupDetails.Email);
            return Ok(new ApiResponse<string>("Signup Successful", true));
        }

        [HttpPost("driver-login")]
        public async Task<IActionResult> DriverLogin([FromBody] LoginDto loginDetails)
        {
            _logger.LogInformation("Driver login attempt for email: {Email}", loginDetails.Email);
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Driver login validation failed for email: {Email}. Errors: {Errors}", loginDetails.Email, string.Join(", ", errorMessages));
                throw new ValidationException("Validation Failed", errorMessages);
            }

            var driver = await _driverService.GetDriverByEmailAsync(loginDetails.Email);
            if (driver == null)
            {
                _logger.LogWarning("Driver login failed: No user found with email: {Email}", loginDetails.Email);
                throw new NotFoundException("User does not exist with this Email");
            }

            if (!_passwordHasher.Verify(loginDetails.Password, driver.PasswordHash))
            {
                _logger.LogWarning("Driver login failed: Invalid password for email: {Email}", loginDetails.Email);
                throw new InvalidCredentialException("Invalid Email or Password");
            }
           
            string token = _tokenService.CreateToken(driver.Id, "Driver");
            _logger.LogInformation("Driver login successful for email: {Email}", loginDetails.Email);

            var responseDto = _mapper.Map<DriverResponseDto>(driver);
            var loginResult = new
            {
                Token = token,
                response = responseDto
            };

            return Ok(new ApiResponse<object>("Login successful", true, loginResult));

        }

        [HttpPost("driver-signup")]
        public async Task<IActionResult> DriverSignup([FromBody] DriverRegisterDto signupDetails)
        {
            _logger.LogInformation("Driver signup attempt with email: {Email}", signupDetails.Email);
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Driver signup validation failed for email: {Email}. Errors: {Errors}", signupDetails.Email, string.Join(", ", errorMessages));
                throw new ValidationException("Validation Failed", errorMessages);
            }
            await _driverService.CreateDriverAsync(signupDetails);
            _logger.LogInformation("Driver signup successful for email: {Email}", signupDetails.Email);
            return Ok(new ApiResponse<string>("Driver Signup Successful", true));
        }

        [HttpGet("is-authenticated")]
        [Authorize]
        public IActionResult isAuthenticated()
        {
            _logger.LogInformation("Is Authenticated Request from {Identuty}", User.Identity);
            if (User.Identity?.IsAuthenticated==true)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("Authenticated user: {UserId}, Role: {Role}", userId, role);

                return Ok(new { isAuthenticated = true, role });
            }
            _logger.LogWarning("Unauthenticated request to /is-authenticated endpoint");
            return Ok(new {isAuthenticated=false});
        }

    }
}
