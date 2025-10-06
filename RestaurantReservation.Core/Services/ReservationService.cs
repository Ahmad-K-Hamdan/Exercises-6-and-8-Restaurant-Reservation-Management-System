using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.DTOs.Reservation;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

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

        public ReservationService(IReservationRepository reservationRepo,
            ICustomerRepository customerRepo,
            IRestaurantRepository restaurantRepo,
            ITableRepository tableRepo,
            IValidator<CreateReservationDTO> createValidator,
            IValidator<UpdateReservationDTO> updateValidator)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;
            _restaurantRepo = restaurantRepo;
            _tableRepo = tableRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<Reservation>> ViewAllAsync()
        {
            return await _reservationRepo.GetAllAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepo.GetByIdAsync(reservationId);
        }

        public async Task<Reservation> AddAsync(CreateReservationDTO dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            ValidateResult(result);

            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId)
                ?? throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found.");
            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            var table = await _tableRepo.GetByIdAsync(dto.TableId)
                ?? throw new KeyNotFoundException($"Table with ID {dto.TableId} not found.");

            var newReservation = new Reservation
            {
                CustomerId = dto.CustomerId,
                RestaurantId = dto.RestaurantId,
                TableId = dto.TableId,
                ReservationDate = dto.ReservationDate,
                PartySize = dto.PartySize
            };

            return await _reservationRepo.AddAsync(newReservation);
        }

        public async Task DeleteAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId)
                ?? throw new KeyNotFoundException($"Reservation with ID {reservationId} not found.");
            await _reservationRepo.DeleteAsync(reservation);
        }

        public async Task<Reservation> UpdateAsync(int reservationId, UpdateReservationDTO dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            ValidateResult(result);

            var reservation = await _reservationRepo.GetByIdAsync(reservationId)
                ?? throw new KeyNotFoundException($"Reservation with ID {reservationId} not found.");

            reservation.CustomerId = dto.CustomerId;
            reservation.RestaurantId = dto.RestaurantId;
            reservation.TableId = dto.TableId;
            reservation.ReservationDate = dto.ReservationDate;
            reservation.PartySize = dto.PartySize;

            return await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task<List<Reservation>> ListReservationsByCustomerAsync(int customerId)
        {
            return await _reservationRepo.GetByCustomerIdAsync(customerId);
        }

        public async Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId)
        {
            return await _reservationRepo.ListOrdersAndMenuItemsAsync(reservationId);
        }

        public async Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId)
        {
            return await _reservationRepo.ListOrderedMenuItemsAsync(reservationId);
        }

        private static void ValidateResult(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                }).ToList();

                var json = JsonSerializer.Serialize(new { errors });
                throw new ArgumentException(json);
            }
        }
    }
}