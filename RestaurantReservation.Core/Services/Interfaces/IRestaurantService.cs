using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<RestaurantDTO> AddAsync(CreateRestaurantDTO dto);
        Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
        Task DeleteAsync(int restaurantId);
        Task<RestaurantDTO> GetRestaurantByIdAsync(int restaurantId);
        Task<RestaurantDTO> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto);
        Task<List<RestaurantDTO>> ViewAllAsync();
    }
}