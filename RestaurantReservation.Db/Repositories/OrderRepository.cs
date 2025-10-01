using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.Employee).ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetByIdAsync(int OrderId)
        {
            return await _context.Orders.Include(o => o.Employee).FirstOrDefaultAsync(o => o.OrderId == OrderId);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByReservationIdAsync(int reservationId)
        {
            return await _context.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            var orders = await _context.Orders.Where(o => o.EmployeeId == employeeId).ToListAsync();
            if (orders.Count == 0)
            {
                return 0;
            }
            return orders.Average(o => o.TotalAmount);
        }
    }
}
