using AutoMapper;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.DTOs.Reservation;

namespace RestaurantReservation.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ITableRepository _tableRepo;
        private readonly IValidator<CreateReservationDTO> _createValidator;
        private readonly IValidator<UpdateReservationDTO> _updateValidator;
        private readonly IMapper _mapper;

        public ReservationService(
            IReservationRepository reservationRepo,
            ICustomerRepository customerRepo,
            IRestaurantRepository restaurantRepo,
            ITableRepository tableRepo,
            IValidator<CreateReservationDTO> createValidator,
            IValidator<UpdateReservationDTO> updateValidator,
            IMapper mapper)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;
            _restaurantRepo = restaurantRepo;
            _tableRepo = tableRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<List<ReservationDTO>> ViewAllAsync()
        {
            var reservations = await _reservationRepo.GetAllAsync();
            return _mapper.Map<List<ReservationDTO>>(reservations);
        }

        public async Task<ReservationDTO> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");
            }

            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> AddAsync(CreateReservationDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {dto.CustomerId} not found.");
            }

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var table = await _tableRepo.GetByIdAsync(dto.TableId);
            if (table == null)
            {
                throw new NotFoundException($"Table with ID {dto.TableId} not found.");
            }

            var newReservation = _mapper.Map<Reservation>(dto);
            var reservation = await _reservationRepo.AddAsync(newReservation);

            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task DeleteAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");
            }
            await _reservationRepo.DeleteAsync(reservation);
        }

        public async Task<ReservationDTO> UpdateAsync(int reservationId, UpdateReservationDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");
            }

            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {dto.CustomerId} not found.");
            }

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var table = await _tableRepo.GetByIdAsync(dto.TableId);
            if (table == null)
            {
                throw new NotFoundException($"Table with ID {dto.TableId} not found.");
            }

            _mapper.Map(dto, reservation);

            var updatedReservation = await _reservationRepo.UpdateAsync(reservation);
            return _mapper.Map<ReservationDTO>(updatedReservation);
        }

        public async Task<List<ReservationDTO>> ListReservationsByCustomerAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }

            var reservations = await _reservationRepo.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<ReservationDTO>>(reservations);
        }

        public async Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");
            }
            return await _reservationRepo.ListOrdersAndMenuItemsAsync(reservationId);
        }

        public async Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");
            }
            return await _reservationRepo.ListOrderedMenuItemsAsync(reservationId);
        }
    }
}