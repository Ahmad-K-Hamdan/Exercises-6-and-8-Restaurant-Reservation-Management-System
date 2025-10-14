using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;

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

        public async Task<List<MenuItemDTO>> ViewAllAsync()
        {
            var menuItems = await _menuItemRepo.GetAllAsync();
            return menuItems.Select(ToDTO).ToList();
        }

        public async Task<MenuItemDTO> GetMenuItemByIdAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {menuItemId} not found.");
            }
            return ToDTO(menuItem);
        }

        public async Task<MenuItemDTO> AddAsync(CreateMenuItemDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var newMenuItem = new MenuItem
            {
                RestaurantId = dto.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            var menuItem = await _menuItemRepo.AddAsync(newMenuItem);
            return ToDTO(menuItem);
        }

        public async Task DeleteAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {menuItemId} not found.");
            }
            await _menuItemRepo.DeleteAsync(menuItem);
        }

        public async Task<MenuItemDTO> UpdateAsync(int menuItemId, UpdateMenuItemDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {menuItemId} not found.");
            }

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;
            menuItem.Restaurant = restaurant;

            var updatedMenuItem = await _menuItemRepo.UpdateAsync(menuItem);
            return ToDTO(updatedMenuItem);
        }

        private static MenuItemDTO ToDTO(MenuItem menuItem)
        {
            return new MenuItemDTO(
                menuItem.ItemId,
                menuItem.Name,
                menuItem.Description,
                menuItem.Price,
                menuItem.RestaurantId,
                menuItem.Restaurant?.Name ?? ""
            );
        }
    }
}