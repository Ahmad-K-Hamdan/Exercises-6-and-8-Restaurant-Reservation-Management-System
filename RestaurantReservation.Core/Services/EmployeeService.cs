using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Employee;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;

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

        public async Task<List<Employee>> ViewAllAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeRepo.GetByIdAsync(employeeId);
        }

        public async Task<Employee> AddAsync(CreateEmployeeDTO dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            ValidateResult(result);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");

            var newEmployee = new Employee
            {
                RestaurantId = dto.RestaurantId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Position = dto.Position
            };

            return await _employeeRepo.AddAsync(newEmployee);
        }

        public async Task DeleteAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId)
                ?? throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            await _employeeRepo.DeleteAsync(employee);
        }

        public async Task<Employee> UpdateAsync(int employeeId, UpdateEmployeeDTO dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            ValidateResult(result);

            var employee = await _employeeRepo.GetByIdAsync(employeeId)
                ?? throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Position = dto.Position;

            return await _employeeRepo.UpdateAsync(employee);
        }

        public async Task<List<Employee>> ListManagersAsync()
        {
            return await _employeeRepo.GetManagersAsync();
        }

        public async Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync()
        {
            return await _employeeRepo.GetEmployeeDetailsAsync();
        }

        private static void ValidateResult(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                }).ToList();

                var json = JsonSerializer.Serialize(new { errors });
                throw new ArgumentException(json);
            }
        }
    }
}