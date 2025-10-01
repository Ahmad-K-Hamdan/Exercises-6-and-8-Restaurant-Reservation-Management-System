namespace RestaurantReservation.API.DTOs.Order
{
    public record CreateOrderDTO(
        int ReservationId,
        int EmployeeId,
        DateTime OrderDate,
        decimal TotalAmount
    );
}