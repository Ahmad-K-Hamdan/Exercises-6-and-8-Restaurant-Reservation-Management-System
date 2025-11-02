using RestaurantReservation.Shared.DTOs.OrderItem;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItemDTO> AddAsync(CreateOrderItemDTO dto);
        Task DeleteAsync(int orderItemId);
        Task<OrderItemDTO> GetOrderItemByIdAsync(int orderItemId);
        Task<OrderItemDTO> UpdateAsync(int orderItemId, UpdateOrderItemDTO dto);
        Task<List<OrderItemDTO>> ViewAllAsync();
    }
}