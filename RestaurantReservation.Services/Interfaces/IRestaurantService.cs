using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<Restaurant> AddAsync(string name, string address, string phoneNumber, string openingHours);
        Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId);
        Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
        Task DeleteAsync(int restaurantId);
        Task<Restaurant> UpdateAsync(int restaurantId, string name, string address, string phoneNumber, string openingHours);
        Task<List<Restaurant>> ViewAllAsync();
    }
}