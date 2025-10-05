namespace RestaurantReservation.API.DTOs.Restaurant
{
    public record UpdateRestaurantDTO(
        string Name,
        string Address,
        string PhoneNumber,
        string OpeningHours
    );
}
