using FluentValidation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Customer;
using RestaurantReservation.Shared.Constants;

namespace RestaurantReservation.Core.Validators.CustomerValidators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDTO>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ValidationMessages.FirstNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.NameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.NameTooLong)
                .Matches(@"^[a-zA-Z]+$").WithMessage(ValidationMessages.NameInvalidCharacters);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ValidationMessages.LastNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.NameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.NameTooLong)
                .Matches(@"^[a-zA-Z]+$").WithMessage(ValidationMessages.NameInvalidCharacters);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.EmailRequired)
                .EmailAddress().WithMessage(ValidationMessages.EmailInvalid)
                .MaximumLength(100).WithMessage(ValidationMessages.EmailMaxLength)
                .MustAsync(BeUniqueEmail).WithMessage(ValidationMessages.EmailAlreadyInUse);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.PhoneNumberRequired)
                .Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage(ValidationMessages.PhoneInvalid);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            return !customers.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}