namespace RestaurantReservation.API.DTOs.Order
{
    public record UpdateOrderDTO(
        int OrderId,
        int ReservationId,
        int EmployeeId,
        DateTime OrderDate,
        decimal TotalAmount
    );
}