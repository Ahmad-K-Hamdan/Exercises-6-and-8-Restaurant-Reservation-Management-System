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

        public async Task<OrderItem> AddAsync(int orderId, int menuItemId, int quantity)
        {
            var order = await GetOrderByIdAsync(orderId);
            var menuItem = await GetMenuItemByIdAsync(menuItemId);

            var quantityValidation = OrderItemValidator.ValidateQuantity(quantity.ToString());
            if (quantityValidation != null)
            {
                throw new ArgumentException(quantityValidation);
            }

            var newOrderItem = new OrderItem
            {
                OrderId = orderId,
                ItemId = menuItemId,
                Quantity = quantity,
                Order = order,
                MenuItem = menuItem
            };

            return await _orderItemRepo.AddAsync(newOrderItem);
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await GetOrderItemByIdAsync(orderItemId);
            await _orderItemRepo.DeleteAsync(orderItem);
        }

        public async Task<OrderItem> UpdateAsync(int orderItemId, int quantity)
        {
            var orderItem = await GetOrderItemByIdAsync(orderItemId);

            var quantityValidation = OrderItemValidator.ValidateQuantity(quantity.ToString());
            if (quantityValidation != null)
            {
                throw new ArgumentException(quantityValidation);
            }

            orderItem.Quantity = quantity;
            return await _orderItemRepo.UpdateAsync(orderItem);
        }

        private async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                throw new InvalidOperationException($"Order item with ID {orderItemId} not found.");
            }
            return orderItem;
        }

        private async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found.");
            }
            return order;
        }

        private async Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
        {
            var menuItem = await _menuItemRepo.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new InvalidOperationException($"Menu item with ID {menuItemId} not found.");
            }
            return menuItem;
        }
    }
}