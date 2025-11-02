namespace RestaurantReservation.API.Auth
{
    public record TokenResponse(
        string Token,
        DateTime ExpiresOn
    );
}