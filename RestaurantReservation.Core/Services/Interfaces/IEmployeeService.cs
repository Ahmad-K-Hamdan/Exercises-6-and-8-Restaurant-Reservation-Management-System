using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.Core.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> AddAsync(CreateEmployeeDTO dto);
        Task DeleteAsync(int employeeId);
        Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeId);
        Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync();
        Task<List<EmployeeDTO>> ListManagersAsync();
        Task<EmployeeDTO> UpdateAsync(int employeeId, UpdateEmployeeDTO dto);
        Task<List<EmployeeDTO>> ViewAllAsync();
    }
}