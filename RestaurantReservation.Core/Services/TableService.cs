using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Table;

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

        public async Task<List<TableDTO>> ViewAllAsync()
        {
            var tables = await _tableRepo.GetAllAsync();
            return tables.Select(ToDTO).ToList();
        }

        public async Task<TableDTO> GetTableByIdAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId);
            if (table == null)
            {
                throw new NotFoundException($"Table with ID {tableId} not found.");
            }
            return ToDTO(table);
        }

        public async Task<TableDTO> AddAsync(CreateTableDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var newTable = new Table
            {
                RestaurantId = dto.RestaurantId,
                Capacity = dto.Capacity,
            };

            var table = await _tableRepo.AddAsync(newTable);
            return ToDTO(table);
        }

        public async Task DeleteAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId);
            if (table == null)
            {
                throw new NotFoundException($"Table with ID {tableId} not found.");
            }
            await _tableRepo.DeleteAsync(table);
        }

        public async Task<TableDTO> UpdateAsync(int tableId, UpdateTableDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var table = await _tableRepo.GetByIdAsync(tableId);
            if (table == null)
            {
                throw new NotFoundException($"Table with ID {tableId} not found.");
            }

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            table.Restaurant = restaurant;
            table.Capacity = dto.Capacity;

            var updatedTable = await _tableRepo.UpdateAsync(table);
            return ToDTO(updatedTable);
        }

        private static TableDTO ToDTO(Table table)
        {
            return new TableDTO(
                table.TableId,
                table.Capacity,
                table.RestaurantId,
                table.Restaurant?.Name ?? ""
            );
        }
    }
}