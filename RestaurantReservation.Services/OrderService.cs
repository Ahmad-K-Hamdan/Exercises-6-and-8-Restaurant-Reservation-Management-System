using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Services
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

        public async Task<Order> AddAsync(int reservationId, int employeeId, DateTime orderDate, decimal totalAmount)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId) ?? throw new ArgumentException($"Reservation with ID {reservationId} not found.");
            var employee = await _employeeRepo.GetByIdAsync(employeeId) ?? throw new ArgumentException($"Employee with ID {employeeId} not found.");
            var orderDateValidation = OrderValidator.ValidateOrderDate(orderDate.ToString("yyyy-MM-dd HH:mm"));
            if (orderDateValidation != null)
            {
                throw new ArgumentException(orderDateValidation);
            }

            var totalAmountValidation = OrderValidator.ValidateTotalAmount(totalAmount.ToString());
            if (totalAmountValidation != null)
            {
                throw new ArgumentException(totalAmountValidation);
            }

            var newOrder = new Order
            {
                ReservationId = reservationId,
                EmployeeId = employeeId,
                OrderDate = orderDate,
                TotalAmount = totalAmount,
                Reservation = reservation,
                Employee = employee
            };

            return await _orderRepo.AddAsync(newOrder);
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            await _orderRepo.DeleteAsync(order);
        }

        public async Task<Order> UpdateAsync(int orderId, int reservationId, int employeeId, DateTime orderDate, decimal totalAmount)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            order.ReservationId = reservationId;
            order.EmployeeId = employeeId;
            order.OrderDate = orderDate;
            order.TotalAmount = totalAmount;

            return await _orderRepo.UpdateAsync(order);
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            return await _orderRepo.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
        }
    }
}