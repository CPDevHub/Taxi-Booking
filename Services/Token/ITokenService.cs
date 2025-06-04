namespace Taxi_Booking.Services.Token
{
    public interface ITokenService
    {
        public string CreateToken(int userId, string role);
    }
}
