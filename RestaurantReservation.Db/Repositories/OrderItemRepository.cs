using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderItemRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.Include(oi => oi.MenuItem).ToListAsync();
        }

        public async Task<OrderItem> AddAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem?> GetByIdAsync(int orderItemId)
        {
            return await _context.OrderItems.Include(oi => oi.MenuItem).FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
        }

        public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
