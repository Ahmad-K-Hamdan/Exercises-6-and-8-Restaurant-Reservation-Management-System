using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetAllAsync();
        Task<OrderItem> AddAsync(OrderItem orderItem);
        Task<OrderItem?> GetByIdAsync(int orderItemId);
        Task<OrderItem> UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(OrderItem orderItem);
    }
}