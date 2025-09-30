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

        public async Task<Reservation> AddAsync(int customerId, int restaurantId, int tableId, DateTime reservationDate, int partySize)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            var restaurant = await GetRestaurantByIdAsync(restaurantId);
            var table = await GetTableByIdAsync(tableId);

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
                PartySize = partySize,
                Customer = customer,
                Restaurant = restaurant,
                Table = table
            };

            return await _reservationRepo.AddAsync(newReservation);
        }

        public async Task DeleteAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            await _reservationRepo.DeleteAsync(reservation);
        }

        public async Task<Reservation> UpdateAsync(int reservationId, DateTime reservationDate, int partySize)
        {
            var reservation = await GetReservationByIdAsync(reservationId);

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

            reservation.ReservationDate = reservationDate;
            reservation.PartySize = partySize;

            return await _reservationRepo.UpdateAsync(reservation);
        }

        public async Task<List<Reservation>> ListReservationsByCustomerAsync(int customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            return await _reservationRepo.GetByCustomerIdAsync(customerId);
        }

        public async Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            return await _reservationRepo.ListOrdersAndMenuItemsAsync(reservationId);
        }

        public async Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            return await _reservationRepo.ListOrderedMenuItemsAsync(reservationId);
        }

        public async Task<List<ReservationDetailsDTO>> GetReservationDetailsAsync()
        {
            return await _reservationRepo.GetReservationDetailsAsync();
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

        private async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer with ID {customerId} not found.");
            }
            return customer;
        }

        private async Task<Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new InvalidOperationException($"Restaurant with ID {restaurantId} not found.");
            }
            return restaurant;
        }

        private async Task<Table> GetTableByIdAsync(int tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId);
            if (table == null)
            {
                throw new InvalidOperationException($"Table with ID {tableId} not found.");
            }
            return table;
        }
    }
}