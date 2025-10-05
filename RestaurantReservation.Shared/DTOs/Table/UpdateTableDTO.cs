namespace RestaurantReservation.API.DTOs.Table
{
    public record UpdateTableDTO(
        int Capacity,
        int RestaurantId
    );
}
