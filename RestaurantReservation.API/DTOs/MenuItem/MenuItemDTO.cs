namespace RestaurantReservation.API.DTOs.MenuItem
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