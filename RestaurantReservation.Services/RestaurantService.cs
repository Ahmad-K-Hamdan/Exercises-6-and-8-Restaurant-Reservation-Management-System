using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Services
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

        public async Task<Restaurant> AddAsync(string name, string address, string phoneNumber, string openingHours)
        {
            var restName = RestaurantValidator.ValidateRestaurantName(name);
            if (restName != null)
            {
                throw new ArgumentException(restName);
            }
            
            var restAddress = RestaurantValidator.ValidateAddress(address);
            if (restAddress != null)
            {
                throw new ArgumentException(restAddress);
            }
            
            var restPhoneNumber = RestaurantValidator.ValidatePhoneNumber(phoneNumber);
            if (restPhoneNumber != null)
            {
                throw new ArgumentException(restPhoneNumber);
            }
            
            var restOpeningHours = RestaurantValidator.ValidateTimeSpan(openingHours);
            if (restOpeningHours != null)
            {
                throw new ArgumentException(restOpeningHours);
            }

            var newRestaurant = new Restaurant
            {
                Name = name,
                Address = address,
                PhoneNumber = phoneNumber,
                OpeningHours = TimeSpan.Parse(openingHours)
            };

            return await _restaurantRepo.AddAsync(newRestaurant);
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            await _restaurantRepo.DeleteAsync(restaurant);
        }

        public async Task<Restaurant> UpdateAsync(int restaurantId, string name, string address, string phoneNumber, string openingHours)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            var restName = RestaurantValidator.ValidateRestaurantName(name);
            if (restName != null)
            {
                throw new ArgumentException(restName);
            }
            
            var restAddress = RestaurantValidator.ValidateAddress(address);
            if (restAddress != null)
            {
                throw new ArgumentException(restAddress);
            }
            
            var restPhoneNumber = RestaurantValidator.ValidatePhoneNumber(phoneNumber);
            if (restPhoneNumber != null)
            {
                throw new ArgumentException(restPhoneNumber);
            }
            
            var restOpeningHours = RestaurantValidator.ValidateTimeSpan(openingHours);
            if (restOpeningHours != null)
            {
                throw new ArgumentException(restOpeningHours);
            }

            restaurant.Name = name;
            restaurant.Address = address;
            restaurant.PhoneNumber = phoneNumber;
            restaurant.OpeningHours = TimeSpan.Parse(openingHours);

            return await _restaurantRepo.UpdateAsync(restaurant);
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            return await _restaurantRepo.GetRestaurantRevenueAsync(restaurantId);
        }
    }
}