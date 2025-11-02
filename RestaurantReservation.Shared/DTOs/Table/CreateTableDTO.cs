namespace RestaurantReservation.Shared.DTOs.Table
{
    public record CreateTableDTO(
        int Capacity,
        int RestaurantId
    );
}
