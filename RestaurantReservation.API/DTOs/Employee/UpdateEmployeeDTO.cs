namespace RestaurantReservation.API.DTOs.Employee
{
    public record UpdateEmployeeDTO(
        int EmployeeId,
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId
    );
}
