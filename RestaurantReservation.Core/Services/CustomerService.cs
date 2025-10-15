using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Customer;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using AutoMapper;

namespace RestaurantReservation.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IValidator<CreateCustomerDTO> _createValidator;
        private readonly IValidator<UpdateCustomerDTO> _updateValidator;
        private readonly IValidator<PartySizeDTO> _partySizeValidator;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepo,
            IValidator<CreateCustomerDTO> createValidator,
            IValidator<UpdateCustomerDTO> updateValidator,
            IValidator<PartySizeDTO> partySizeValidator,
            IMapper mapper)
        {
            _customerRepo = customerRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _partySizeValidator = partySizeValidator;
            _mapper = mapper;
        }

        public async Task<List<CustomerDTO>> ViewAllAsync()
        {
            var customers = await _customerRepo.GetAllAsync();
            return _mapper.Map<List<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> AddAsync(CreateCustomerDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var newCustomer = _mapper.Map<Customer>(dto);

            var customer = await _customerRepo.AddAsync(newCustomer);
            return _mapper.Map<CustomerDTO>(customer);
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

            _mapper.Map(dto, customer);

            var updatedCustomer = await _customerRepo.UpdateAsync(customer);
            return _mapper.Map<CustomerDTO>(updatedCustomer);
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
    }
}