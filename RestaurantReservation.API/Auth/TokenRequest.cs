namespace RestaurantReservation.API.Auth
{
    public record TokenRequest(
        string Username,
        string Email
    );
}