using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.DTOs;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> ViewAllAsync();
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<Reservation> AddAsync(int customerId, int restaurantId, int tableId, DateTime reservationDate, int partySize);
        Task DeleteAsync(int reservationId);
        Task<Reservation> UpdateAsync(int reservationId, int customerId, int restaurantId, int tableId, DateTime reservationDate, int partySize);
        Task<List<Reservation>> ListReservationsByCustomerAsync(int customerId);
        Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId);
        Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId);
    }
}