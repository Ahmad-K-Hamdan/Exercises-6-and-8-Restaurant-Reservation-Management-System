using FluentValidation;
using RestaurantReservation.Shared.Constants;
using RestaurantReservation.Shared.DTOs.Employee;
using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Core.Validators.EmployeeValidators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDTO>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ValidationMessages.FirstNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.NameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.NameTooLong)
                .Matches(RegexPatterns.PersonName).WithMessage(ValidationMessages.NameInvalidCharacters);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ValidationMessages.LastNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.NameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.NameTooLong)
                .Matches(RegexPatterns.PersonName).WithMessage(ValidationMessages.NameInvalidCharacters);

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage(ValidationMessages.PositionRequired)
                .Must(value => Enum.TryParse<EmployeePosition>(value, true, out _))
                .WithMessage(ValidationMessages.PositionInvalid);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);
        }
    }
}