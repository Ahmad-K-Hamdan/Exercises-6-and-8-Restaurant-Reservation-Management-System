using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateRestaurantDTO> _createValidator;
        private readonly IValidator<UpdateRestaurantDTO> _updateValidator;

        public RestaurantService(IRestaurantRepository restaurantRepo,
            IValidator<CreateRestaurantDTO> createValidator,
            IValidator<UpdateRestaurantDTO> updateValidator)
        {
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<RestaurantDTO>> ViewAllAsync()
        {
            var restaurants = await _restaurantRepo.GetAllAsync();
            return restaurants.Select(ToDTO).ToList();
        }

        public async Task<RestaurantDTO> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            return ToDTO(restaurant);
        }

        public async Task<RestaurantDTO> AddAsync(CreateRestaurantDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var newRestaurant = new Restaurant
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                OpeningHours = TimeSpan.Parse(dto.OpeningHours)
            };

            var restaurant = await _restaurantRepo.AddAsync(newRestaurant);
            return ToDTO(restaurant);
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            await _restaurantRepo.DeleteAsync(restaurant);
        }

        public async Task<RestaurantDTO> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }

            restaurant.Name = dto.Name;
            restaurant.Address = dto.Address;
            restaurant.PhoneNumber = dto.PhoneNumber;
            restaurant.OpeningHours = TimeSpan.Parse(dto.OpeningHours);

            var updatedRestaurant = await _restaurantRepo.UpdateAsync(restaurant);
            return ToDTO(updatedRestaurant);
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            return await _restaurantRepo.GetRestaurantRevenueAsync(restaurantId);
        }

        private static RestaurantDTO ToDTO(Restaurant restaurant)
        {
            return new RestaurantDTO(
                restaurant.RestaurantId,
                restaurant.Name,
                restaurant.Address,
                restaurant.PhoneNumber,
                restaurant.OpeningHours
            );
        }
    }
}