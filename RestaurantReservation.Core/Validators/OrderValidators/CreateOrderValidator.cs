using FluentValidation;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.Constants;

namespace RestaurantReservation.Core.Validators.OrderValidators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDTO>
    {
        private readonly IReservationRepository _reservationRepository;

        public CreateOrderValidator(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

            RuleFor(x => x.ReservationId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidReservationIdRequired);

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage(ValidationMessages.ValidEmployeeIdRequired);

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage(ValidationMessages.OrderDateRequired)
                .LessThanOrEqualTo(DateTime.Now).WithMessage(ValidationMessages.DateCannotBeFuture)
                .GreaterThanOrEqualTo(DateTime.Now.AddDays(-1)).WithMessage(ValidationMessages.OrderDateTooOld);

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage(ValidationMessages.TotalAmountLessThanOrEqualToZero)
                .LessThanOrEqualTo(10000).WithMessage(ValidationMessages.TotalAmountTooHigh);

            RuleFor(x => x)
                .MustAsync(OrderDateMatchesReservationDate)
                .WithMessage(ValidationMessages.OrderDateMustMatchReservation)
                .WithName(nameof(CreateOrderDTO.OrderDate));
        }

        private async Task<bool> OrderDateMatchesReservationDate(CreateOrderDTO dto, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(dto.ReservationId);
            if (reservation == null) return true;

            return dto.OrderDate.Date == reservation.ReservationDate.Date;
        }
    }
}