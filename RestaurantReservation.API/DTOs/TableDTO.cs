namespace RestaurantReservation.API.DTOs
{
    public record TableDTO(
        int TableId,
        int Capacity,
        int RestaurantId,
        string RestaurantName
    );
}
