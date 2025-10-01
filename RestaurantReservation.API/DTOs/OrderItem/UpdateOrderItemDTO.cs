namespace RestaurantReservation.API.DTOs.OrderItem
{
    public record UpdateOrderItemDTO(
        int OrderItemId,
        int OrderId,
        int ItemId,
        int Quantity
    );
}