namespace RestaurantReservation.Shared.DTOs.MenuItem
{
    public record MenuItemDTO(
        int ItemId,
        string Name,
        string Description,
        decimal Price,
        int RestaurantId,
        string RestaurantName
    );
}