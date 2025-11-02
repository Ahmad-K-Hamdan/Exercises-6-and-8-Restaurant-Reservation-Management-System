using FluentValidation;
using RestaurantReservation.Shared.Constants;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Validators.TableValidators
{
    public class UpdateTableValidator : AbstractValidator<UpdateTableDTO>
    {
        public UpdateTableValidator()
        {
            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.CapacityLessThanOne)
                .LessThanOrEqualTo(50).WithMessage(ValidationMessages.CapacityTooHigh);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);
        }
    }
}