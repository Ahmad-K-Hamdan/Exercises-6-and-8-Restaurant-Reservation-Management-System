using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.OrderItem;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> ViewAllAsync();
        Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId);
        Task<OrderItem> AddAsync(CreateOrderItemDTO dto);
        Task DeleteAsync(int orderItemId);
        Task<OrderItem> UpdateAsync(int orderItemId, UpdateOrderItemDTO dto);
    }
}