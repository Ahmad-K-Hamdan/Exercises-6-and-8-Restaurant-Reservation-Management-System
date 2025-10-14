using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Customer;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IValidator<CreateCustomerDTO> _createValidator;
        private readonly IValidator<UpdateCustomerDTO> _updateValidator;
        private readonly IValidator<PartySizeDTO> _partySizeValidator;

        public CustomerService(ICustomerRepository customerRepo,
            IValidator<CreateCustomerDTO> createValidator,
            IValidator<UpdateCustomerDTO> updateValidator,
            IValidator<PartySizeDTO> partySizeValidator)
        {
            _customerRepo = customerRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _partySizeValidator = partySizeValidator;
        }

        public async Task<List<CustomerDTO>> ViewAllAsync()
        {
            var customers = await _customerRepo.GetAllAsync();
            return customers.Select(ToDTO).ToList();
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }
            return ToDTO(customer);
        }

        public async Task<CustomerDTO> AddAsync(CreateCustomerDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var newCustomer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var customer = await _customerRepo.AddAsync(newCustomer);
            return ToDTO(customer);
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }
            await _customerRepo.DeleteAsync(customer);
        }

        public async Task<CustomerDTO> UpdateAsync(int customerId, UpdateCustomerDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;

            var updatedCustomer = await _customerRepo.UpdateAsync(customer);
            return ToDTO(updatedCustomer);
        }

        public async Task<List<CustomerDetailsDTO>> FindCustomersByPartySizeAsync(int minPartySize)
        {
            var dto = new PartySizeDTO { PartySize = minPartySize };
            await _partySizeValidator.ValidateAndThrowAsync(dto);

            var customers = await _customerRepo.FindCustomersByPartySizeAsync(minPartySize);
            if (customers == null || !customers.Any())
            {
                throw new NotFoundException($"No customers found with party size >= {minPartySize}.");
            }

            return customers;
        }

        private static CustomerDTO ToDTO(Customer customer)
        {
            return new CustomerDTO(
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber
            );
        }
    }
}