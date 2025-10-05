namespace RestaurantReservation.Shared.DTOs.Restaurant
{
    public record RestaurantDTO(
        int RestaurantId,
        string Name,
        string Address,
        string PhoneNumber,
        TimeSpan OpeningHours
    );
}
