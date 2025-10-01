using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> ViewAllAsync();
        Task<Order> AddAsync(int reservationId, int employeeId, DateTime orderDate, decimal totalAmount);
        Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId);
        Task DeleteAsync(int orderId);
        Task<Order> UpdateAsync(int orderId, int reservationId, int employeeId, DateTime orderDate, decimal totalAmount);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}