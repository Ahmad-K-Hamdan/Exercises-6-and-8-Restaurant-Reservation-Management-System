using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.DTOs.Reservation;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> ViewAllAsync();
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<Reservation> AddAsync(CreateReservationDTO dto);
        Task DeleteAsync(int reservationId);
        Task<Reservation> UpdateAsync(int reservationId, UpdateReservationDTO dto);
        Task<List<Reservation>> ListReservationsByCustomerAsync(int customerId);
        Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId);
        Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId);
    }
}