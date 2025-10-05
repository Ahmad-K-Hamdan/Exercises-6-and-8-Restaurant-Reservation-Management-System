using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepo;
        private readonly IRestaurantRepository _restaurantRepo;

        public TableService(ITableRepository tableRepo, IRestaurantRepository restaurantRepo)
        {
            _tableRepo = tableRepo;
            _restaurantRepo = restaurantRepo;
        }

        public async Task<List<Table>> ViewAllAsync()
        {
            return await _tableRepo.GetAllAsync();
        }

        public async Task<Table?> GetTableByIdAsync(int tableId)
        {
            return await _tableRepo.GetByIdAsync(tableId);
        }

        public async Task<Table> AddAsync(CreateTableDTO dto)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId) ?? throw new ArgumentException($"Restaurant with ID {dto.RestaurantId} not found.");

            var newTable = new Table
            {
                RestaurantId = dto.RestaurantId,
                Capacity = dto.Capacity,
            };

            return await _tableRepo.AddAsync(newTable);
        }

        public async Task DeleteAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new ArgumentException($"Table with ID {tableId} not found.");
            await _tableRepo.DeleteAsync(table);
        }

        public async Task<Table> UpdateAsync(int tableId, UpdateTableDTO dto)
        {
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new ArgumentException($"Table with ID {tableId} not found.");

            table.RestaurantId = dto.RestaurantId;
            table.Capacity = dto.Capacity;

            return await _tableRepo.UpdateAsync(table);
        }
    }
}