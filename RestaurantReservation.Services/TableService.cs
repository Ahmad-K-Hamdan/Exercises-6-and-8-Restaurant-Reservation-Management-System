using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Services
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

        public async Task<Table> AddAsync(int restaurantId, int capacity)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            var tableCapacity = TableValidator.ValidateCapacity(capacity.ToString());
            if (tableCapacity != null)
            {
                throw new ArgumentException(tableCapacity);
            }

            var newTable = new Table
            {
                RestaurantId = restaurantId,
                Capacity = capacity,
                Restaurant = restaurant
            };

            return await _tableRepo.AddAsync(newTable);
        }

        public async Task DeleteAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new ArgumentException($"Table with ID {tableId} not found.");
            await _tableRepo.DeleteAsync(table);
        }

        public async Task<Table> UpdateAsync(int tableId, int restaurantId, int capacity)
        {
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new ArgumentException($"Table with ID {tableId} not found.");
            var tableCapacity = TableValidator.ValidateCapacity(capacity.ToString());
            if (tableCapacity != null)
            {
                throw new ArgumentException(tableCapacity);
            }

            table.RestaurantId = restaurantId;
            table.Capacity = capacity;

            return await _tableRepo.UpdateAsync(table);
        }
    }
}