namespace RestaurantReservation.API.DTOs.Restaurant
{
    public record RestaurantDTO(
        int RestaurantId,
        string Name,
        string Address,
        string PhoneNumber,
        TimeSpan OpeningHours
    );
}
