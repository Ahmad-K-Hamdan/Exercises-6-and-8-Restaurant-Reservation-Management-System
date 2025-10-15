using FluentValidation;
using RestaurantReservation.Shared.Constants;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Core.Validators.CustomerValidators
{
    public class PartySizeValidator : AbstractValidator<PartySizeDTO>
    {
        public PartySizeValidator()
        {
            RuleFor(x => x.PartySize)
                .GreaterThan(0).WithMessage(ValidationMessages.PartySizeLessThanOrEqualToZero)
                .LessThanOrEqualTo(50).WithMessage(ValidationMessages.PartySizeTooHigh);
        }
    }
}