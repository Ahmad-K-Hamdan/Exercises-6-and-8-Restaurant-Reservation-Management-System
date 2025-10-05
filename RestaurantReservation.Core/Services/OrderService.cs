using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IEmployeeRepository _employeeRepo;

        public OrderService(IOrderRepository orderRepo, IReservationRepository reservationRepo, IEmployeeRepository employeeRepo)
        {
            _orderRepo = orderRepo;
            _reservationRepo = reservationRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task<List<Order>> ViewAllAsync()
        {
            return await _orderRepo.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepo.GetByIdAsync(orderId);
        }

        public async Task<Order> AddAsync(CreateOrderDTO dto)
        {
            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId) ?? throw new ArgumentException($"Reservation with ID {dto.ReservationId} not found.");
            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId) ?? throw new ArgumentException($"Employee with ID {dto.EmployeeId} not found.");

            var newOrder = new Order
            {
                ReservationId = dto.ReservationId,
                EmployeeId = dto.EmployeeId,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount
            };

            return await _orderRepo.AddAsync(newOrder);
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            await _orderRepo.DeleteAsync(order);
        }

        public async Task<Order> UpdateAsync(int orderId, UpdateOrderDTO dto)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");

            order.ReservationId = dto.ReservationId;
            order.EmployeeId = dto.EmployeeId;
            order.OrderDate = dto.OrderDate;
            order.TotalAmount = dto.TotalAmount;

            return await _orderRepo.UpdateAsync(order);
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            return await _orderRepo.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
        }
    }
}