using AutoMapper;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IValidator<CreateOrderDTO> _createValidator;
        private readonly IValidator<UpdateOrderDTO> _updateValidator;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepo,
            IReservationRepository reservationRepo,
            IEmployeeRepository employeeRepo,
            IValidator<CreateOrderDTO> createValidator,
            IValidator<UpdateOrderDTO> updateValidator,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _reservationRepo = reservationRepo;
            _employeeRepo = employeeRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<List<OrderDTO>> ViewAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> AddAsync(CreateOrderDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {dto.ReservationId} not found.");
            }

            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {dto.EmployeeId} not found.");
            }

            var newOrder = _mapper.Map<Order>(dto);
            var order = await _orderRepo.AddAsync(newOrder);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }
            await _orderRepo.DeleteAsync(order);
        }

        public async Task<OrderDTO> UpdateAsync(int orderId, UpdateOrderDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }

            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {dto.ReservationId} not found.");
            }

            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {dto.EmployeeId} not found.");
            }

            _mapper.Map(dto, order);

            var updatedOrder = await _orderRepo.UpdateAsync(order);
            return _mapper.Map<OrderDTO>(updatedOrder);
        }

        public async Task<decimal> CalculateAverageOrderAmountByEmployeeAsync(int employeeId)
        {
            return await _orderRepo.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
        }
    }
}