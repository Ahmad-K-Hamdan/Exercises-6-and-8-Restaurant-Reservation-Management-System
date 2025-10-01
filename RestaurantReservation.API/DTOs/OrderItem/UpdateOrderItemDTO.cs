namespace RestaurantReservation.API.DTOs.OrderItem
{
    public record UpdateOrderItemDTO(
        int OrderId,
        int ItemId,
        int Quantity
    );
}