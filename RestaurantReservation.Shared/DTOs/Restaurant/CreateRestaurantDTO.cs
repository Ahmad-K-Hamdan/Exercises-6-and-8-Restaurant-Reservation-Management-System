namespace RestaurantReservation.Shared.DTOs.Restaurant
{
    public record CreateRestaurantDTO(
        string Name,
        string Address,
        string PhoneNumber,
        string OpeningHours
    );
}
