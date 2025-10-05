using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;

        public RestaurantService(IRestaurantRepository restaurantRepo)
        {
            _restaurantRepo = restaurantRepo;
        }

        public async Task<List<Restaurant>> ViewAllAsync()
        {
            return await _restaurantRepo.GetAllAsync();
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId)
        {
            return await _restaurantRepo.GetByIdAsync(restaurantId);
        }

        public async Task<Restaurant> AddAsync(CreateRestaurantDTO dto)
        {

            var newRestaurant = new Restaurant
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                OpeningHours = TimeSpan.Parse(dto.OpeningHours)
            };

            return await _restaurantRepo.AddAsync(newRestaurant);
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            await _restaurantRepo.DeleteAsync(restaurant);
        }

        public async Task<Restaurant> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");

            restaurant.Name = dto.Name;
            restaurant.Address = dto.Address;
            restaurant.PhoneNumber = dto.PhoneNumber;
            restaurant.OpeningHours = TimeSpan.Parse(dto.OpeningHours);

            return await _restaurantRepo.UpdateAsync(restaurant);
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            return await _restaurantRepo.GetRestaurantRevenueAsync(restaurantId);
        }
    }
}