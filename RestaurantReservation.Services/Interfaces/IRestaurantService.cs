using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> ViewAllAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId);
        Task<Restaurant> AddAsync(string name, string address, string phoneNumber, string openingHours);
        Task DeleteAsync(int restaurantId);
        Task<Restaurant> UpdateAsync(int restaurantId, string name, string address, string phoneNumber, string openingHours);
        Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
    }
}