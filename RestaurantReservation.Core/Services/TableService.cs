using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Table;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

namespace RestaurantReservation.Core.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateTableDTO> _createValidator;
        private readonly IValidator<UpdateTableDTO> _updateValidator;

        public TableService(ITableRepository tableRepo,
            IRestaurantRepository restaurantRepo,
            IValidator<CreateTableDTO> createValidator,
            IValidator<UpdateTableDTO> updateValidator)
        {
            _tableRepo = tableRepo;
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
            var result = await _createValidator.ValidateAsync(dto);
            ValidateResult(result);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId) ?? throw new KeyNotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");

            var newTable = new Table
            {
                RestaurantId = dto.RestaurantId,
                Capacity = dto.Capacity,
            };

            return await _tableRepo.AddAsync(newTable);
        }

        public async Task DeleteAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new KeyNotFoundException($"Table with ID {tableId} not found.");
            await _tableRepo.DeleteAsync(table);
        }

        public async Task<Table> UpdateAsync(int tableId, UpdateTableDTO dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            ValidateResult(result);

            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new KeyNotFoundException($"Table with ID {tableId} not found.");

            table.RestaurantId = dto.RestaurantId;
            table.Capacity = dto.Capacity;

            return await _tableRepo.UpdateAsync(table);
        }

        private static void ValidateResult(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                }).ToList();

                var json = JsonSerializer.Serialize(new { errors });
                throw new ArgumentException(json);
            }
        }
    }
}