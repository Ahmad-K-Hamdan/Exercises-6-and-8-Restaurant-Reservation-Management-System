namespace RestaurantReservation.API.DTOs.Restaurant
{
    public record CreateRestaurantDTO(
        string Name,
        string Address,
        string PhoneNumber,
        string OpeningHours
    );
}
