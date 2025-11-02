using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.DTOs.Reservation;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDTO> AddAsync(CreateReservationDTO dto);
        Task DeleteAsync(int reservationId);
        Task<ReservationDTO> GetReservationByIdAsync(int reservationId);
        Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId);
        Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId);
        Task<List<ReservationDTO>> ListReservationsByCustomerAsync(int customerId);
        Task<ReservationDTO> UpdateAsync(int reservationId, UpdateReservationDTO dto);
        Task<List<ReservationDTO>> ViewAllAsync();
    }
}