using FluentValidation;
using RestaurantReservation.Core.Constants;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.Core.Validators.EmployeeValidators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDTO>
    {
        private static readonly string[] ValidPositions = { "Manager", "Server", "Chef", "Host", "Bartender", "Cashier" };

        public CreateEmployeeValidator()
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

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage(ValidationMessages.PositionRequired)
                .Must(p => ValidPositions.Contains(p, StringComparer.OrdinalIgnoreCase))
                .WithMessage(ValidationMessages.PositionInvalid);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);
        }
    }
}