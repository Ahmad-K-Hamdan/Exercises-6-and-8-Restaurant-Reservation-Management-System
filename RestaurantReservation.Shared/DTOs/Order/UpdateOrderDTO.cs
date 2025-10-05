namespace RestaurantReservation.Shared.DTOs.Order
{
    public record UpdateOrderDTO(
        int ReservationId,
        int EmployeeId,
        DateTime OrderDate,
        decimal TotalAmount
    );
}