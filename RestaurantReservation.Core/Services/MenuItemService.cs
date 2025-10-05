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

        public MenuItemService(IMenuItemRepository menuItemRepo, IRestaurantRepository restaurantRepo)
        {
            _menuItemRepo = menuItemRepo;
            _restaurantRepo = restaurantRepo;
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
            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId) ?? throw new ArgumentException($"Restaurant with ID {dto.RestaurantId} not found.");

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
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId) ?? throw new ArgumentException($"Menu item with ID {menuItemId} not found.");
            await _menuItemRepo.DeleteAsync(menuItem);
        }

        public async Task<MenuItem> UpdateAsync(int menuItemId, UpdateMenuItemDTO dto)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId) ?? throw new ArgumentException($"Menu item with ID {menuItemId} not found.");

            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;

            return await _menuItemRepo.UpdateAsync(menuItem);
        }
    }
}