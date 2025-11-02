namespace RestaurantReservation.Shared.DTOs.MenuItem
{
    public record UpdateMenuItemDTO(
        string Name,
        string Description,
        decimal Price,
        int RestaurantId
    );
}