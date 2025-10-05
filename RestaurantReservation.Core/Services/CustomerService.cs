using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Customer;
using FluentValidation;
using FluentValidation.Results;

namespace RestaurantReservation.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IValidator<CreateCustomerDTO> _createValidator;
        private readonly IValidator<UpdateCustomerDTO> _updateValidator;
        private readonly IValidator<PartySizeDTO> _partySizeValidator;

        public CustomerService(ICustomerRepository customerRepo, IValidator<CreateCustomerDTO> createValidator, IValidator<UpdateCustomerDTO> updateValidator, IValidator<PartySizeDTO> partySizeValidator)
        {
            _customerRepo = customerRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _partySizeValidator = partySizeValidator;
        }

        public async Task<List<Customer>> ViewAllAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepo.GetByIdAsync(customerId);
        }

        public async Task<Customer> AddAsync(CreateCustomerDTO dto)
        {
            ValidationResult result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                throw new ArgumentException(string.Join("\n", result.Errors.Select(e => e.ErrorMessage)));
            }

            var newCustomer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            return await _customerRepo.AddAsync(newCustomer);
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId) ?? throw new ArgumentException($"Customer with ID {customerId} not found.");
            await _customerRepo.DeleteAsync(customer);
        }

        public async Task<Customer> UpdateAsync(int customerId, UpdateCustomerDTO dto)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId)
                ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            ValidationResult result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                throw new ArgumentException(string.Join("\n", result.Errors.Select(e => e.ErrorMessage)));
            }

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;

            return await _customerRepo.UpdateAsync(customer);
        }

        public async Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize)
        {
            var dto = new PartySizeDTO { PartySize = minPartySize };

            ValidationResult result = await _partySizeValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                throw new ArgumentException(string.Join("\n", result.Errors.Select(e => e.ErrorMessage)));
            }

            return await _customerRepo.FindCustomersByPartySizeAsync(minPartySize);
        }
    }
}