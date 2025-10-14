using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Employee;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateEmployeeDTO> _createValidator;
        private readonly IValidator<UpdateEmployeeDTO> _updateValidator;

        public EmployeeService(IEmployeeRepository employeeRepo,
            IRestaurantRepository restaurantRepo,
            IValidator<CreateEmployeeDTO> createValidator,
            IValidator<UpdateEmployeeDTO> updateValidator)
        {
            _employeeRepo = employeeRepo;
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<EmployeeDTO>> ViewAllAsync()
        {
            var employees = await _employeeRepo.GetAllAsync();
            return employees.Select(ToDTO).ToList();
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {employeeId} not found.");
            }
            return ToDTO(employee);
        }

        public async Task<EmployeeDTO> AddAsync(CreateEmployeeDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var newEmployee = new Employee
            {
                RestaurantId = dto.RestaurantId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Position = dto.Position
            };

            var employee = await _employeeRepo.AddAsync(newEmployee);
            return ToDTO(employee);
        }

        public async Task DeleteAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {employeeId} not found.");
            }
            await _employeeRepo.DeleteAsync(employee);
        }

        public async Task<EmployeeDTO> UpdateAsync(int employeeId, UpdateEmployeeDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {employeeId} not found.");
            }

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Position = dto.Position;
            employee.Restaurant = restaurant;

            var updatedEmployee = await _employeeRepo.UpdateAsync(employee);
            return ToDTO(updatedEmployee);
        }

        public async Task<List<EmployeeDTO>> ListManagersAsync()
        {
            var managers = await _employeeRepo.GetManagersAsync();
            return managers.Select(ToDTO).ToList();
        }

        public async Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync()
        {
            return await _employeeRepo.GetEmployeeDetailsAsync();
        }

        private static EmployeeDTO ToDTO(Employee employee)
        {
            return new EmployeeDTO(
                employee.EmployeeId,
                employee.FirstName,
                employee.LastName,
                employee.Position,
                employee.RestaurantId,
                employee.Restaurant?.Name ?? ""
            );
        }
    }
}