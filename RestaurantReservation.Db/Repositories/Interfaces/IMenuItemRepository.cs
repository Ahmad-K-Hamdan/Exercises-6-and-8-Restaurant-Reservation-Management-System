using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetAllAsync();
        Task<MenuItem> AddAsync(MenuItem menuItem);
        Task<MenuItem?> GetByIdAsync(int itemId);
        Task<MenuItem> UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(MenuItem menuItem);
    }
}