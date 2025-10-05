namespace RestaurantReservation.API.DTOs.Employee
{
    public record CreateEmployeeDTO(
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId
    );
}
