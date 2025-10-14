using FluentValidation;
using RestaurantReservation.Core.Exceptions;
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
        private readonly IValidator<CreateOrderItemDTO> _createValidator;
        private readonly IValidator<UpdateOrderItemDTO> _updateValidator;

        public OrderItemService(IOrderItemRepository orderItemRepo,
            IOrderRepository orderRepo,
            IMenuItemRepository menuItemRepo,
            IValidator<CreateOrderItemDTO> createValidator,
            IValidator<UpdateOrderItemDTO> updateValidator)
        {
            _orderItemRepo = orderItemRepo;
            _orderRepo = orderRepo;
            _menuItemRepo = menuItemRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<OrderItemDTO>> ViewAllAsync()
        {
            var orderItems = await _orderItemRepo.GetAllAsync();
            return orderItems.Select(ToDTO).ToList();
        }

        public async Task<OrderItemDTO> GetOrderItemByIdAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                throw new NotFoundException($"Order item with ID {orderItemId} not found.");
            }
            return ToDTO(orderItem);
        }

        public async Task<OrderItemDTO> AddAsync(CreateOrderItemDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {dto.OrderId} not found.");
            }

            var menuItem = await _menuItemRepo.GetByIdAsync(dto.ItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {dto.ItemId} not found.");
            }

            var newOrderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ItemId = dto.ItemId,
                Quantity = dto.Quantity
            };

            var orderItem = await _orderItemRepo.AddAsync(newOrderItem);
            return ToDTO(orderItem);
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                throw new NotFoundException($"Order item with ID {orderItemId} not found.");
            }
            await _orderItemRepo.DeleteAsync(orderItem);
        }

        public async Task<OrderItemDTO> UpdateAsync(int orderItemId, UpdateOrderItemDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var orderItem = await _orderItemRepo.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                throw new NotFoundException($"Order item with ID {orderItemId} not found.");
            }

            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {dto.OrderId} not found.");
            }

            var menuItem = await _menuItemRepo.GetByIdAsync(dto.ItemId);
            if (menuItem == null)
            {
                throw new NotFoundException($"Menu item with ID {dto.ItemId} not found.");
            }

            orderItem.Order = order;
            orderItem.MenuItem = menuItem;
            orderItem.Quantity = dto.Quantity;

            var updatedOrderItem = await _orderItemRepo.UpdateAsync(orderItem);
            return ToDTO(updatedOrderItem);
        }

        private static OrderItemDTO ToDTO(OrderItem orderItem)
        {
            return new OrderItemDTO(
                orderItem.OrderItemId,
                orderItem.OrderId,
                orderItem.ItemId,
                orderItem.Quantity,
                orderItem.MenuItem?.Name ?? ""
            );
        }
    }
}