using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> ViewAllAsync();
        Task<Order> AddAsync(CreateOrderDTO dto);
        Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId);
        Task DeleteAsync(int orderId);
        Task<Order> UpdateAsync(int orderId, UpdateOrderDTO dto);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}