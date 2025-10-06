using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

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
            var result = await _createValidator.ValidateAsync(dto);
            ValidateResult(result);

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
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");
            await _restaurantRepo.DeleteAsync(restaurant);
        }

        public async Task<Restaurant> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            ValidateResult(result);

            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            restaurant.Name = dto.Name;
            restaurant.Address = dto.Address;
            restaurant.PhoneNumber = dto.PhoneNumber;
            restaurant.OpeningHours = TimeSpan.Parse(dto.OpeningHours);

            return await _restaurantRepo.UpdateAsync(restaurant);
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");
            return await _restaurantRepo.GetRestaurantRevenueAsync(restaurantId);
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