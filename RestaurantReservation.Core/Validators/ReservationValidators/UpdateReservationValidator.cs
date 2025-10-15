using FluentValidation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Reservation;
using RestaurantReservation.Shared.Constants;

namespace RestaurantReservation.Core.Validators.ReservationValidators
{
    public class UpdateReservationValidator : AbstractValidator<UpdateReservationDTO>
    {
        private readonly ITableRepository _tableRepository;

        public UpdateReservationValidator(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidCustomerIdRequired);

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidRestaurantIdRequired);

            RuleFor(x => x.TableId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidTableIdRequired);

            RuleFor(x => x.ReservationDate)
                .NotEmpty().WithMessage(ValidationMessages.ReservationDateRequired)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ValidationMessages.DateCannotBePast)
                .LessThanOrEqualTo(DateTime.Now.AddYears(1)).WithMessage(ValidationMessages.ReservationDateTooFar);

            RuleFor(x => x.PartySize)
                .GreaterThan(0).WithMessage(ValidationMessages.PartySizeLessThanOrEqualToZero)
                .LessThanOrEqualTo(50).WithMessage(ValidationMessages.PartySizeTooHigh);

            RuleFor(x => x)
                .MustAsync(HaveSufficientTableCapacity)
                .WithMessage(ValidationMessages.PartySizeExceedsTableCapacity)
                .WithName(nameof(UpdateReservationDTO.PartySize));
        }

        private async Task<bool> HaveSufficientTableCapacity(UpdateReservationDTO dto, CancellationToken cancellationToken)
        {
            var table = await _tableRepository.GetByIdAsync(dto.TableId);
            if (table == null) return true;

            return dto.PartySize <= table.Capacity;
        }
    }
}