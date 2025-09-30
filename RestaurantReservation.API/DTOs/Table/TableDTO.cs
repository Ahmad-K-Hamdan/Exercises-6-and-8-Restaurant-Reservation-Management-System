namespace RestaurantReservation.API.DTOs.Table
{
    public record TableDTO(
        int TableId,
        int Capacity,
        int RestaurantId,
        string RestaurantName
    );
}
