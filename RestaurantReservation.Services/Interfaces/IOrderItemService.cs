using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItem> AddAsync(int orderId, int menuItemId, int quantity);
        Task<List<OrderItem>> ViewAllAsync();
        Task DeleteAsync(int orderItemId);
        Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId);
        Task<OrderItem> UpdateAsync(int orderItemId, int orderId, int itemId, int quantity);
    }
}