using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface ITableService
    {
        Task<List<Table>> ViewAllAsync();
        Task<Table?> GetTableByIdAsync(int tableId);
        Task<Table> AddAsync(CreateTableDTO dto);
        Task DeleteAsync(int tableId);
        Task<Table> UpdateAsync(int tableId, UpdateTableDTO dto);
    }
}