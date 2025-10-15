using FluentValidation;
using RestaurantReservation.Shared.Constants;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Core.Validators.CustomerValidators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDTO>
    {
        public UpdateCustomerValidator()
        {
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
                .MaximumLength(100).WithMessage(ValidationMessages.EmailMaxLength);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.PhoneNumberRequired)
                .Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage(ValidationMessages.PhoneInvalid);
        }
    }
}