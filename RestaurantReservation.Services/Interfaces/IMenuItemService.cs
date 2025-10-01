using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<List<MenuItem>> ViewAllAsync();
        Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
        Task<MenuItem> AddAsync(int restaurantId, string name, string description, decimal price);
        Task DeleteAsync(int menuItemId);
        Task<MenuItem> UpdateAsync(int menuItemId, string name, string description, decimal price);
    }
}