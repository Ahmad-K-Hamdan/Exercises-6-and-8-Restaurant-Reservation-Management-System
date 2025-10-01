using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services
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

        public async Task<MenuItem> AddAsync(int restaurantId, string name, string description, decimal price)
        {
            var restaurant = await GetRestaurantByIdAsync(restaurantId);

            var menuItemName = MenuItemValidator.ValidateMenuItemName(name);
            if (menuItemName != null)
            {
                throw new ArgumentException(menuItemName);
            }
            var menuItemDescription = MenuItemValidator.ValidateDescription(description);
            if (menuItemDescription != null)
            {
                throw new ArgumentException(menuItemDescription);
            }
            var menuItemPrice = MenuItemValidator.ValidatePrice(price.ToString());
            if (menuItemPrice != null)
            {
                throw new ArgumentException(menuItemPrice);
            }

            var newMenuItem = new MenuItem
            {
                RestaurantId = restaurantId,
                Name = name,
                Description = description,
                Price = price,
                Restaurant = restaurant
            };

            return await _menuItemRepo.AddAsync(newMenuItem);
        }

        public async Task DeleteAsync(int menuItemId)
        {
            var menuItem = await FindMenuItemByIdAsync(menuItemId);
            await _menuItemRepo.DeleteAsync(menuItem);
        }

        public async Task<MenuItem> UpdateAsync(int menuItemId, string name, string description, decimal price)
        {
            var menuItem = await FindMenuItemByIdAsync(menuItemId);

            var menuItemName = MenuItemValidator.ValidateMenuItemName(name);
            if (menuItemName != null)
            {
                throw new ArgumentException(menuItemName);
            }
            var menuItemDescription = MenuItemValidator.ValidateDescription(description);
            if (menuItemDescription != null)
            {
                throw new ArgumentException(menuItemDescription);
            }
            var menuItemPrice = MenuItemValidator.ValidatePrice(price.ToString());
            if (menuItemPrice != null)
            {
                throw new ArgumentException(menuItemPrice);
            }

            menuItem.Name = name;
            menuItem.Description = description;
            menuItem.Price = price;

            return await _menuItemRepo.UpdateAsync(menuItem);
        }

        private async Task<MenuItem> FindMenuItemByIdAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new InvalidOperationException($"Menu item with ID {menuItemId} not found.");
            }
            return menuItem;
        }

        private async Task<Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new InvalidOperationException($"Restaurant with ID {restaurantId} not found.");
            }
            return restaurant;
        }
    }
}