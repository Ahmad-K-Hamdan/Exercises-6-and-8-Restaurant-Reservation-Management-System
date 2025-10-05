using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;

namespace RestaurantReservation.Db.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public ReservationRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations.Include(res => res.Restaurant).Include(res => res.Customer).ToListAsync();
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation?> GetByIdAsync(int ReservationId)
        {
            return await _context.Reservations.Include(res => res.Restaurant).Include(res => res.Customer).FirstOrDefaultAsync(r => r.ReservationId == ReservationId);
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetByCustomerIdAsync(int CustomerId)
        {
            return await _context.Reservations.Include(res => res.Restaurant).Where(res => res.CustomerId == CustomerId).ToListAsync();
        }

        public async Task<List<OrderWithItemsDTO>> ListOrdersAndMenuItemsAsync(int reservationId)
        {
            return await _context.Orders
                .Where(o => o.ReservationId == reservationId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Select(o => new OrderWithItemsDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    ReservationId = o.ReservationId,
                    Items = o.OrderItems.Select(oi => new OrderedMenuItemDTO
                    {
                        MenuItemName = oi.MenuItem!.Name,
                        Price = oi.MenuItem.Price,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<OrderedMenuItemDTO>> ListOrderedMenuItemsAsync(int reservationId)
        {
            return await _context.OrderItems.Where(oi => oi.Order!.ReservationId == reservationId)
                    .Include(oi => oi.MenuItem)
                    .Include(oi => oi.Order)
                    .Select(oi => new OrderedMenuItemDTO
                    {
                        MenuItemName = oi.MenuItem!.Name,
                        Price = oi.MenuItem.Price,
                        Quantity = oi.Quantity,
                    }).ToListAsync();
        }
    }
}
