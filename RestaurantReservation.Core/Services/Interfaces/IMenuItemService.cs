using RestaurantReservation.Shared.DTOs.MenuItem;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<MenuItemDTO> AddAsync(CreateMenuItemDTO dto);
        Task DeleteAsync(int menuItemId);
        Task<MenuItemDTO> GetMenuItemByIdAsync(int menuItemId);
        Task<MenuItemDTO> UpdateAsync(int menuItemId, UpdateMenuItemDTO dto);
        Task<List<MenuItemDTO>> ViewAllAsync();
    }
}