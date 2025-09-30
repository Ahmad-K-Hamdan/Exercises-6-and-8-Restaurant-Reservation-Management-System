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

        public async Task<Order> AddAsync(int reservationId, int employeeId, DateTime orderDate, decimal totalAmount)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            var employee = await GetEmployeeByIdAsync(employeeId);

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
            var order = await GetOrderByIdAsync(orderId);
            await _orderRepo.DeleteAsync(order);
        }

        public async Task<Order> UpdateAsync(int orderId, int employeeId)
        {
            var order = await GetOrderByIdAsync(orderId);
            order.EmployeeId = employeeId;
            return await _orderRepo.UpdateAsync(order);
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            var employee = await GetEmployeeByIdAsync(employeeId);
            return await _orderRepo.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
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

        private async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new InvalidOperationException($"Reservation with ID {reservationId} not found.");
            }
            return reservation;
        }

        private async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
            }
            return employee;
        }
    }
}