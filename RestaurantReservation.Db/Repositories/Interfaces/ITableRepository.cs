using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface ITableRepository
    {
        Task<List<Table>> GetAllAsync();
        Task<Table> AddAsync(Table table);
        Task<Table?> GetByIdAsync(int tableId);
        Task<Table> UpdateAsync(Table table);
        Task DeleteAsync(Table table);
    }
}