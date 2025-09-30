using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface ITableService
    {
        Task<List<Table>> ViewAllAsync();
        Task<Table> AddAsync(int restaurantId, int capacity);
        Task DeleteAsync(int tableId);
        Task<Table> UpdateAsync(int tableId, int restaurantId, int capacity);
        Task<Table> GetTableByIdAsync(int tableId);
    }
}