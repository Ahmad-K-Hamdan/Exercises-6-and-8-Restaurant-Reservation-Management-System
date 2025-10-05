using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Validation;
using RestaurantReservation.Core.DTOs;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IRestaurantRepository _restaurantRepo;

        public EmployeeService(IEmployeeRepository employeeRepo, IRestaurantRepository restaurantRepo)
        {
            _employeeRepo = employeeRepo;
            _restaurantRepo = restaurantRepo;
        }

        public async Task<List<Employee>> ViewAllAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeRepo.GetByIdAsync(employeeId);
        }

        public async Task<Employee> AddAsync(int restaurantId, string firstName, string lastName, string position)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId) ?? throw new ArgumentException($"Restaurant with ID {restaurantId} not found.");
            var empFirstName = EmployeeValidator.ValidateFirstName(firstName);
            if (empFirstName != null)
            {
                throw new ArgumentException(empFirstName);
            }

            var empLastName = EmployeeValidator.ValidateLastName(lastName);
            if (empLastName != null)
            {
                throw new ArgumentException(empLastName);
            }

            var empPosition = EmployeeValidator.ValidatePosition(position);
            if (empPosition != null)
            {
                throw new ArgumentException(empPosition);
            }

            var newEmployee = new Employee
            {
                RestaurantId = restaurantId,
                FirstName = firstName,
                LastName = lastName,
                Position = position
            };

            return await _employeeRepo.AddAsync(newEmployee);
        }

        public async Task DeleteAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId) ?? throw new ArgumentException($"Employee with ID {employeeId} not found.");
            await _employeeRepo.DeleteAsync(employee);
        }

        public async Task<Employee> UpdateAsync(int employeeId, string firstName, string lastName, string position)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId) ?? throw new ArgumentException($"Employee with ID {employeeId} not found.");
            var empFirstName = EmployeeValidator.ValidateFirstName(firstName);
            if (empFirstName != null)
            {
                throw new ArgumentException(empFirstName);
            }

            var empLastName = EmployeeValidator.ValidateLastName(lastName);
            if (empLastName != null)
            {
                throw new ArgumentException(empLastName);
            }

            var empPosition = EmployeeValidator.ValidatePosition(position);
            if (empPosition != null)
            {
                throw new ArgumentException(empPosition);
            }

            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Position = position;

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
    }
}