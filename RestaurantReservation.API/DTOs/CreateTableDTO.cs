namespace RestaurantReservation.API.DTOs
{
    public record CreateTableDTO(
        int Capacity,
        int RestaurantId
    );
}
