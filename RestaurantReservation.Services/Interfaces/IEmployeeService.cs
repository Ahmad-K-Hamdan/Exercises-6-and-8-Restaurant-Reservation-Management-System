using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.DTOs;

namespace RestaurantReservation.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> ViewAllAsync();
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> AddAsync(int restaurantId, string firstName, string lastName, string position);
        Task DeleteAsync(int employeeId);
        Task<Employee> UpdateAsync(int employeeId, string firstName, string lastName, string position);
        Task<List<Employee>> ListManagersAsync();
        Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync();
    }
}