using FluentValidation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.Constants;

namespace RestaurantReservation.Core.Validators.MenuItemValidators
{
    public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemDTO>
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public CreateMenuItemValidator(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.MenuItemNameRequired)
                .MinimumLength(2).WithMessage(ValidationMessages.MenuItemNameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.MenuItemNameTooLong);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationMessages.DescriptionRequired)
                .MinimumLength(10).WithMessage(ValidationMessages.DescriptionTooShort)
                .MaximumLength(500).WithMessage(ValidationMessages.DescriptionTooLong);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(ValidationMessages.PriceLessThanOrEqualToZero)
                .LessThanOrEqualTo(1000).WithMessage(ValidationMessages.PriceTooHigh);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);

            RuleFor(x => x)
                .MustAsync(HaveReasonablePrice)
                .WithMessage(ValidationMessages.PriceNotReasonable)
                .WithName(nameof(CreateMenuItemDTO.Price));
        }

        private async Task<bool> HaveReasonablePrice(CreateMenuItemDTO dto, CancellationToken cancellationToken)
        {
            var allMenuItems = await _menuItemRepository.GetAllAsync();
            var restaurantItems = allMenuItems.Where(m => m.RestaurantId == dto.RestaurantId).ToList();

            if (!restaurantItems.Any()) return true;

            var avgPrice = restaurantItems.Average(m => m.Price);

            return dto.Price >= avgPrice * 0.2m && dto.Price <= avgPrice * 5m;
        }
    }
}