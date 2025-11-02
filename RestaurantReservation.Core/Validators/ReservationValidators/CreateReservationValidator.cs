using FluentValidation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Reservation;
using RestaurantReservation.Shared.Constants;

namespace RestaurantReservation.Core.Validators.ReservationValidators
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationDTO>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;

        public CreateReservationValidator(IReservationRepository reservationRepository, ITableRepository tableRepository)
        {
            _reservationRepository = reservationRepository;
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
                .WithName(nameof(CreateReservationDTO.PartySize));

            RuleFor(x => x)
                .MustAsync(NotHaveReservationConflict)
                .WithMessage(ValidationMessages.TableAlreadyReserved)
                .WithName(nameof(CreateReservationDTO.ReservationDate));
        }

        private async Task<bool> HaveSufficientTableCapacity(CreateReservationDTO dto, CancellationToken cancellationToken)
        {
            var table = await _tableRepository.GetByIdAsync(dto.TableId);
            if (table == null) return true;

            return dto.PartySize <= table.Capacity;
        }

        private async Task<bool> NotHaveReservationConflict(CreateReservationDTO dto, CancellationToken cancellationToken)
        {
            var allReservations = await _reservationRepository.GetAllAsync();

            var conflictingReservations = allReservations.Where(r =>
                r.TableId == dto.TableId &&
                Math.Abs((r.ReservationDate - dto.ReservationDate).TotalHours) < 2
            );

            return !conflictingReservations.Any();
        }
    }
}