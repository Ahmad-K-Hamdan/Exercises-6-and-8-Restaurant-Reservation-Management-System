using FluentValidation;
using RestaurantReservation.Core.Constants;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Validators.TableValidators
{
    public class CreateTableValidator : AbstractValidator<CreateTableDTO>
    {
        public CreateTableValidator()
        {
            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.CapacityLessThanOne)
                .LessThanOrEqualTo(50).WithMessage(ValidationMessages.CapacityTooHigh);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);
        }
    }
}