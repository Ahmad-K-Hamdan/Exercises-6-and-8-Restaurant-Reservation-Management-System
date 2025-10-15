using AutoMapper;
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
        private readonly IMapper _mapper;

        public MenuItemService(
            IMenuItemRepository menuItemRepo,
            IRestaurantRepository restaurantRepo,
            IValidator<CreateMenuItemDTO> createValidator,
            IValidator<UpdateMenuItemDTO> updateValidator,
            IMapper mapper)
        {
            _menuItemRepo = menuItemRepo;
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<List<MenuItemDTO>> ViewAllAsync()
        {
            var menuItems = await _menuItemRepo.GetAllAsync();
            return _mapper.Map<List<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO> GetMenuItemByIdAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {menuItemId} not found.");
            }
            return _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<MenuItemDTO> AddAsync(CreateMenuItemDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var newMenuItem = _mapper.Map<MenuItem>(dto);

            var menuItem = await _menuItemRepo.AddAsync(newMenuItem);
            return _mapper.Map<MenuItemDTO>(menuItem);
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

            _mapper.Map(dto, menuItem);

            var updatedMenuItem = await _menuItemRepo.UpdateAsync(menuItem);
            return _mapper.Map<MenuItemDTO>(updatedMenuItem);
        }
    }
}
