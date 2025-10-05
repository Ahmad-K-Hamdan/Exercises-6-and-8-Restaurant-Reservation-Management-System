using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<List<MenuItem>> ViewAllAsync();
        Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
        Task<MenuItem> AddAsync(CreateMenuItemDTO dto);
        Task DeleteAsync(int menuItemId);
        Task<MenuItem> UpdateAsync(int menuItemId, UpdateMenuItemDTO dto);
    }
}