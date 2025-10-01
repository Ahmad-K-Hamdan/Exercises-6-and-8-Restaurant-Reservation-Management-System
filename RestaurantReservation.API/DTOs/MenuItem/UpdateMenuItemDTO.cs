namespace RestaurantReservation.API.DTOs.MenuItem
{
    public record UpdateMenuItemDTO(
        int ItemId,
        string Name,
        string Description,
        decimal Price,
        int RestaurantId
    );
}