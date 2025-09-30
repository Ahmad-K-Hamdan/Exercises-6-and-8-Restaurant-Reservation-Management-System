namespace RestaurantReservation.API.DTOs
{
    public record CreateEmployeeDTO(
        string FirstName,
        string LastName,
        string Position,
        int RestaurantId
    );
}
