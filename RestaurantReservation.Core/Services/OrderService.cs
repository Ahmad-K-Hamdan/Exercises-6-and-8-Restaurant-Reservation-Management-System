using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Order;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IValidator<CreateOrderDTO> _createValidator;
        private readonly IValidator<UpdateOrderDTO> _updateValidator;

        public OrderService(IOrderRepository orderRepo,
            IReservationRepository reservationRepo,
            IEmployeeRepository employeeRepo,
            IValidator<CreateOrderDTO> createValidator,
            IValidator<UpdateOrderDTO> updateValidator)
        {
            _orderRepo = orderRepo;
            _reservationRepo = reservationRepo;
            _employeeRepo = employeeRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<OrderDTO>> ViewAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(ToDTO).ToList();
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }
            return ToDTO(order);
        }

        public async Task<OrderDTO> AddAsync(CreateOrderDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {dto.ReservationId} not found.");
            }

            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {dto.EmployeeId} not found.");
            }

            var newOrder = new Order
            {
                ReservationId = dto.ReservationId,
                EmployeeId = dto.EmployeeId,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount
            };

            var order = await _orderRepo.AddAsync(newOrder);
            return ToDTO(order);
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }
            await _orderRepo.DeleteAsync(order);
        }

        public async Task<OrderDTO> UpdateAsync(int orderId, UpdateOrderDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {dto.ReservationId} not found.");
            }

            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {dto.EmployeeId} not found.");
            }

            order.Reservation = reservation;
            order.Employee = employee;
            order.OrderDate = dto.OrderDate;
            order.TotalAmount = dto.TotalAmount;

            var updatedOrder = await _orderRepo.UpdateAsync(order);
            return ToDTO(updatedOrder);
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            return await _orderRepo.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
        }

        private static OrderDTO ToDTO(Order order)
        {
            return new OrderDTO(
                order.OrderId,
                order.OrderDate,
                order.TotalAmount,
                order.ReservationId,
                order.EmployeeId,
                order.Employee != null ? $"{order.Employee.FirstName} {order.Employee.LastName}" : ""
            );
        }
    }
}