using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDTO> AddAsync(CreateCustomerDTO dto);
        Task DeleteAsync(int customerId);
        Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize);
        Task<CustomerDTO> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> UpdateAsync(int customerId, UpdateCustomerDTO dto);
        Task<List<CustomerDTO>> ViewAllAsync();
    }
}