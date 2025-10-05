using FluentValidation;
using RestaurantReservation.Core.Constants;
using RestaurantReservation.Shared.DTOs.OrderItem;

namespace RestaurantReservation.Core.Validators.OrderItemValidators
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDTO>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidOrderIdRequired);

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidItemIdRequired);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(ValidationMessages.QuantityLessThanOrEqualToZero)
                .LessThanOrEqualTo(100).WithMessage(ValidationMessages.QuantityTooHigh);
        }
    }
}