using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services
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

        public async Task<OrderItem> AddAsync(int orderId, int menuItemId, int quantity)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId) ?? throw new ArgumentException($"Menu item with ID {menuItemId} not found.");
            var quantityValidation = OrderItemValidator.ValidateQuantity(quantity.ToString());
            if (quantityValidation != null)
            {
                throw new ArgumentException(quantityValidation);
            }

            var newOrderItem = new OrderItem
            {
                OrderId = orderId,
                ItemId = menuItemId,
                Quantity = quantity
            };

            return await _orderItemRepo.AddAsync(newOrderItem);
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId) ?? throw new ArgumentException($"Order item with ID {orderItemId} not found.");
            await _orderItemRepo.DeleteAsync(orderItem);
        }

        public async Task<OrderItem> UpdateAsync(int orderItemId, int orderId, int itemId, int quantity)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId) ?? throw new ArgumentException($"Order item with ID {orderItemId} not found.");
            var quantityValidation = OrderItemValidator.ValidateQuantity(quantity.ToString());
            if (quantityValidation != null)
            {
                throw new ArgumentException(quantityValidation);
            }

            orderItem.OrderId = orderId;
            orderItem.ItemId = itemId;
            orderItem.Quantity = quantity;

            return await _orderItemRepo.UpdateAsync(orderItem);
        }
    }
}