namespace RestaurantReservation.API.DTOs
{
    public record UpdateEmployeeDTO(
        int EmployeeId,
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId
    );
}
