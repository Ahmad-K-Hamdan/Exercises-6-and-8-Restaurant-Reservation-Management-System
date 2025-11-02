using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation?> GetByIdAsync(int reservationId);
        Task<Reservation> UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task<List<Reservation>> GetByCustomerIdAsync(int customerId);
        Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId);
        Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId);
    }
}