using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> ViewAllAsync();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer> AddAsync(CreateCustomerDTO dto);
        Task DeleteAsync(int customerId);
        Task<Customer> UpdateAsync(int customerId, UpdateCustomerDTO dto);
        Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize);
    }
}