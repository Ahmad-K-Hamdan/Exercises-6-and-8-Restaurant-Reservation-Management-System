using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int customerId);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize);
    }
}