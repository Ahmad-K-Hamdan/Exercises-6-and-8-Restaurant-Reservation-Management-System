namespace RestaurantReservation.Shared.DTOs.OrderItem
{
    public record CreateOrderItemDTO(
        int OrderId,
        int ItemId,
        int Quantity
    );
}