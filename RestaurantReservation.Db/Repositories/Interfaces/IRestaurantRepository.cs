using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAllAsync();
        Task<Restaurant> AddAsync(Restaurant restaurant);
        Task<Restaurant?> GetByIdAsync(int restaurantId);
        Task<Restaurant> UpdateAsync(Restaurant restaurant);
        Task DeleteAsync(Restaurant restaurant);
        Task<decimal> GetRestaurantRevenueAsync(int restaurantId);
    }
}