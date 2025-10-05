namespace RestaurantReservation.Shared.DTOs.Table
{
    public record UpdateTableDTO(
        int Capacity,
        int RestaurantId
    );
}
