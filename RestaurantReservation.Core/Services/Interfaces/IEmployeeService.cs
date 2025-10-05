using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> ViewAllAsync();
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> AddAsync(CreateEmployeeDTO dto);
        Task DeleteAsync(int employeeId);
        Task<Employee> UpdateAsync(int employeeId, UpdateEmployeeDTO dto);
        Task<List<Employee>> ListManagersAsync();
        Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync();
    }
}