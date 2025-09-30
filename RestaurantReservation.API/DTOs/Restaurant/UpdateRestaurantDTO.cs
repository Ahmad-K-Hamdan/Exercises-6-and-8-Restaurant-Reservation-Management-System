namespace RestaurantReservation.API.DTOs.Restaurant
{
    public record UpdateRestaurantDTO(
        int RestaurantId,
        string Name,
        string Address,
        string PhoneNumber,
        string OpeningHours
    );
}
