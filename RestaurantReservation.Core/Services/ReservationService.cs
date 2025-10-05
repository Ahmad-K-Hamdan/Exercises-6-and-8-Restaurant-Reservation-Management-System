using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Core.DTOs;
using RestaurantReservation.Services.Interfaces;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ITableRepository _tableRepo;

        public ReservationService(IReservationRepository reservationRepo, ICustomerRepository customerRepo, IRestaurantRepository restaurantRepo, ITableRepository tableRepo)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;
            _restaurantRepo = restaurantRepo;
            _tableRepo = tableRepo;
        }

        public async Task<List<Reservation>> ViewAllAsync()
        {
            return await _reservationRepo.GetAllAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepo.GetByIdAsync(reservationId);
        }

        public async Task<Reservation> AddAsync(int customerId, int restaurantId, int tableId, DateTime reservationDate, int partySize)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId) ?? throw new ArgumentException($"Customer with ID {customerId} not found.");
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            var table = await _tableRepo.GetByIdAsync(tableId) ?? throw new ArgumentException($"Table with ID {tableId} not found.");
            var reservationDateValidation = ReservationValidator.ValidateReservationDate(reservationDate.ToString("yyyy-MM-dd HH:mm"));
            if (reservationDateValidation != null)
            {
                throw new ArgumentException(reservationDateValidation);
            }

            var partySizeValidation = ReservationValidator.ValidatePartySize(partySize.ToString());
            if (partySizeValidation != null)
            {
                throw new ArgumentException(partySizeValidation);
            }

            var newReservation = new Reservation
            {
                CustomerId = customerId,
                RestaurantId = restaurantId,
                TableId = tableId,
                ReservationDate = reservationDate,
                PartySize = partySize
            };

            return await _reservationRepo.AddAsync(newReservation);
        }

        public async Task DeleteAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId) ?? throw new ArgumentException($"Reservation with ID {reservationId} not found.");
            await _reservationRepo.DeleteAsync(reservation);
        }

        public async Task<Reservation> UpdateAsync(int reservationId, int customerId, int restaurantId, int tableId, DateTime reservationDate, int partySize)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId) ?? throw new ArgumentException($"Reservation with ID {reservationId} not found.");
            var reservationDateValidation = ReservationValidator.ValidateReservationDate(reservationDate.ToString("yyyy-MM-dd HH:mm"));
            if (reservationDateValidation != null)
            {
                throw new ArgumentException(reservationDateValidation);
            }

            var partySizeValidation = ReservationValidator.ValidatePartySize(partySize.ToString());
            if (partySizeValidation != null)
            {
                throw new ArgumentException(partySizeValidation);
            }

            reservation.CustomerId = customerId;
            reservation.RestaurantId = restaurantId;
            reservation.TableId = tableId;
            reservation.ReservationDate = reservationDate;
            reservation.PartySize = partySize;

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
    }
}