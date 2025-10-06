using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

namespace RestaurantReservation.Core.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateMenuItemDTO> _createValidator;
        private readonly IValidator<UpdateMenuItemDTO> _updateValidator;

        public MenuItemService(IMenuItemRepository menuItemRepo,
            IRestaurantRepository restaurantRepo,
            IValidator<CreateMenuItemDTO> createValidator,
            IValidator<UpdateMenuItemDTO> updateValidator)
        {
            _menuItemRepo = menuItemRepo;
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<MenuItem>> ViewAllAsync()
        {
            return await _menuItemRepo.GetAllAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
        {
            return await _menuItemRepo.GetByIdAsync(menuItemId);
        }

        public async Task<MenuItem> AddAsync(CreateMenuItemDTO dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            ValidateResult(result);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");

            var newMenuItem = new MenuItem
            {
                RestaurantId = dto.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            return await _menuItemRepo.AddAsync(newMenuItem);
        }

        public async Task DeleteAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId)
                ?? throw new KeyNotFoundException($"Menu item with ID {menuItemId} not found.");
            await _menuItemRepo.DeleteAsync(menuItem);
        }

        public async Task<MenuItem> UpdateAsync(int menuItemId, UpdateMenuItemDTO dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            ValidateResult(result);

            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId)
                ?? throw new KeyNotFoundException($"Menu item with ID {menuItemId} not found.");

            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;

            return await _menuItemRepo.UpdateAsync(menuItem);
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