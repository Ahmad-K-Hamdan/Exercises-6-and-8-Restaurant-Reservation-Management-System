using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> AddAsync(Order order);
        Task<Order?> GetByIdAsync(int orderId);
        Task<Order> UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<List<Order>> GetOrdersByReservationIdAsync(int reservationId);
        Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId);
    }
}