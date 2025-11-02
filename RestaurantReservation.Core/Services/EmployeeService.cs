using AutoMapper;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateEmployeeDTO> _createValidator;
        private readonly IValidator<UpdateEmployeeDTO> _updateValidator;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepo,
            IRestaurantRepository restaurantRepo,
            IValidator<CreateEmployeeDTO> createValidator,
            IValidator<UpdateEmployeeDTO> updateValidator,
            IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDTO>> ViewAllAsync()
        {
            var employees = await _employeeRepo.GetAllAsync();
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {employeeId} not found.");
            }
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> AddAsync(CreateEmployeeDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {dto.RestaurantId} not found.");
            }

            var newEmployee = _mapper.Map<Employee>(dto);

            var employee = await _employeeRepo.AddAsync(newEmployee);
            return _mapper.Map<EmployeeDTO>(employee);
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

            _mapper.Map(dto, employee);

            var updatedEmployee = await _employeeRepo.UpdateAsync(employee);
            return _mapper.Map<EmployeeDTO>(updatedEmployee);
        }

        public async Task<List<EmployeeDTO>> ListManagersAsync()
        {
            var managers = await _employeeRepo.GetManagersAsync();
            return _mapper.Map<List<EmployeeDTO>>(managers);
        }

        public async Task<List<EmployeeDetailsDTO>> GetEmployeeDetailsAsync()
        {
            return await _employeeRepo.GetEmployeeDetailsAsync();
        }
    }
}