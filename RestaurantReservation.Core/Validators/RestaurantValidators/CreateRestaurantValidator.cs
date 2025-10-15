using FluentValidation;
using RestaurantReservation.Shared.Constants;
using RestaurantReservation.Shared.DTOs.Restaurant;
using System.Text.RegularExpressions;

namespace RestaurantReservation.Core.Validators.RestaurantValidators
{
    public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantDTO>
    {
        public CreateRestaurantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.RestaurantNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.RestaurantNameTooShort)
                .MaximumLength(100).WithMessage(ValidationMessages.RestaurantNameTooLong);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(ValidationMessages.AddressRequired)
                .MinimumLength(5).WithMessage(ValidationMessages.AddressTooShort)
                .MaximumLength(200).WithMessage(ValidationMessages.AddressTooLong);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.PhoneNumberRequired)
                .Matches(RegexPatterns.Phone).WithMessage(ValidationMessages.PhoneInvalid);

            RuleFor(x => x.OpeningHours)
                .NotEmpty().WithMessage(ValidationMessages.OpeningHoursRequired)
                .Must(BeValidOpeningHoursFormat).WithMessage(ValidationMessages.OpeningHoursInvalidFormat)
                .Must(HaveValidOpeningHoursRange).WithMessage(ValidationMessages.OpeningHoursInvalidRange);
        }

        private bool BeValidOpeningHoursFormat(string openingHours)
        {
            if (string.IsNullOrWhiteSpace(openingHours)) return false;

            var regex = new Regex(@"^([0-1][0-9]|2[0-3]):[0-5][0-9]-([0-1][0-9]|2[0-3]):[0-5][0-9]$");
            return regex.IsMatch(openingHours);
        }

        private bool HaveValidOpeningHoursRange(string openingHours)
        {
            if (string.IsNullOrWhiteSpace(openingHours)) return false;

            var parts = openingHours.Split('-');
            if (parts.Length != 2) return false;

            if (TimeSpan.TryParse(parts[0], out var openTime) &&
                TimeSpan.TryParse(parts[1], out var closeTime))
            {
                return openTime < closeTime;
            }

            return false;
        }
    }
}