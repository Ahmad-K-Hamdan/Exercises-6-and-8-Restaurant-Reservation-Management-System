namespace RestaurantReservation.API.DTOs.Order
{
    public record OrderDTO(
        int OrderId,
        DateTime OrderDate,
        decimal TotalAmount,
        int ReservationId,
        int EmployeeId,
        string EmployeeName
    );
}