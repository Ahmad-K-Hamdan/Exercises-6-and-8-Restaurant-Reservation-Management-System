using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.DTOs;

namespace RestaurantReservation.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> ViewAllAsync();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer> AddAsync(string firstName, string lastName, string email, string phoneNumber);
        Task DeleteAsync(int customerId);
        Task<Customer> UpdateAsync(int customerId, string firstName, string lastName, string email, string phoneNumber);
        Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize);
    }
}