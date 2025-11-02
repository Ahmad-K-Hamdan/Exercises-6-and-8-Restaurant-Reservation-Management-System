using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public RestaurantRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> AddAsync(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant?> GetByIdAsync(int RestaurantId)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(rest => rest.RestaurantId == RestaurantId);
        }

        public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task DeleteAsync(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetRestaurantRevenueAsync(int restaurantId)
        {
            try
            {
                var revenue = await _context.Database
                    .SqlQuery<decimal>($"SELECT dbo.CalcTotalRevenue({restaurantId}) AS Value")
                    .FirstOrDefaultAsync();
                return revenue;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
