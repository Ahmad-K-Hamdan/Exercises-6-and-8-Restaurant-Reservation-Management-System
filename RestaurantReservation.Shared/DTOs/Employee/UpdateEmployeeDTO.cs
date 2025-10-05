namespace RestaurantReservation.Shared.DTOs.Employee
{
    public record UpdateEmployeeDTO(
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId
    );
}
