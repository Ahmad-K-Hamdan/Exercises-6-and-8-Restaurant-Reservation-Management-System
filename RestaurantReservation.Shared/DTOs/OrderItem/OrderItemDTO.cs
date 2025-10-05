namespace RestaurantReservation.API.DTOs.OrderItem
{
    public record OrderItemDTO(
        int OrderItemId,
        int OrderId,
        int ItemId,
        int Quantity,
        string MenuItemName
    );
}