using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> ViewAllAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId);
        Task<Restaurant> AddAsync(CreateRestaurantDTO dto);
        Task DeleteAsync(int restaurantId);
        Task<Restaurant> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto);
        Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
    }
}