namespace RestaurantReservation.Shared.DTOs.MenuItem
{
    public record CreateMenuItemDTO(
        string Name,
        string Description,
        decimal Price,
        int RestaurantId
    );
}