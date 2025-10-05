using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.OrderItem;

namespace RestaurantReservation.Core.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuItemRepository _menuItemRepo;

        public OrderItemService(IOrderItemRepository orderItemRepo, IOrderRepository orderRepo, IMenuItemRepository menuItemRepo)
        {
            _orderItemRepo = orderItemRepo;
            _orderRepo = orderRepo;
            _menuItemRepo = menuItemRepo;
        }

        public async Task<List<OrderItem>> ViewAllAsync()
        {
            return await _orderItemRepo.GetAllAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId)
        {
            return await _orderItemRepo.GetByIdAsync(orderItemId);
        }

        public async Task<OrderItem> AddAsync(CreateOrderItemDTO dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId) ?? throw new ArgumentException($"Order with ID {dto.OrderId} not found.");
            var menuItem = await _menuItemRepo.GetByIdAsync(dto.ItemId) ?? throw new ArgumentException($"Menu item with ID {dto.ItemId} not found.");

            var newOrderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ItemId = dto.ItemId,
                Quantity = dto.Quantity
            };

            return await _orderItemRepo.AddAsync(newOrderItem);
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId) ?? throw new ArgumentException($"Order item with ID {orderItemId} not found.");
            await _orderItemRepo.DeleteAsync(orderItem);
        }

        public async Task<OrderItem> UpdateAsync(int orderItemId, UpdateOrderItemDTO dto)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId) ?? throw new ArgumentException($"Order item with ID {orderItemId} not found.");

            orderItem.OrderId = dto.OrderId;
            orderItem.ItemId = dto.ItemId;
            orderItem.Quantity = dto.Quantity;

            return await _orderItemRepo.UpdateAsync(orderItem);
        }
    }
}