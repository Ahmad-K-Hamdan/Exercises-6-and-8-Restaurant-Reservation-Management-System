namespace RestaurantReservation.API.DTOs.Table
{
    public record CreateTableDTO(
        int Capacity,
        int RestaurantId
    );
}
