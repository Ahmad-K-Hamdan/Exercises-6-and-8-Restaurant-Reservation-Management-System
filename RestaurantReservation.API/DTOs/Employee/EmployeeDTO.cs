namespace RestaurantReservation.API.DTOs.Employee
{
    public record EmployeeDTO(
        int EmployeeId,
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId,
        string RestaurantName
    );
}
