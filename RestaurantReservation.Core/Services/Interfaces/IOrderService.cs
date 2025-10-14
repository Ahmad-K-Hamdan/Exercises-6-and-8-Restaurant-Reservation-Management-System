using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> AddAsync(CreateOrderDTO dto);
        Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId);
        Task DeleteAsync(int orderId);
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<OrderDTO> UpdateAsync(int orderId, UpdateOrderDTO dto);
        Task<List<OrderDTO>> ViewAllAsync();
    }
}