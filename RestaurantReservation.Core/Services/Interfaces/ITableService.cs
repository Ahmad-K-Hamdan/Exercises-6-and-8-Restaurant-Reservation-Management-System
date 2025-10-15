using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface ITableService
    {
        Task<TableDTO> AddAsync(CreateTableDTO dto);
        Task DeleteAsync(int tableId);
        Task<TableDTO> GetTableByIdAsync(int tableId);
        Task<TableDTO> UpdateAsync(int tableId, UpdateTableDTO dto);
        Task<List<TableDTO>> ViewAllAsync();
    }
}