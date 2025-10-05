namespace RestaurantReservation.Shared.DTOs.Restaurant
{
    public record UpdateRestaurantDTO(
        string Name,
        string Address,
        string PhoneNumber,
        string OpeningHours
    );
}
