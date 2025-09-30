namespace RestaurantReservation.API.DTOs
{
    public record UpdateTableDTO(
        int TableId,
        int Capacity,
        int RestaurantId
    );
}
