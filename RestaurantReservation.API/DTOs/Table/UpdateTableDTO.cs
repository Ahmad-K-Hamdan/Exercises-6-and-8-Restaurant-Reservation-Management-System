namespace RestaurantReservation.API.DTOs.Table
{
    public record UpdateTableDTO(
        int TableId,
        int Capacity,
        int RestaurantId
    );
}
