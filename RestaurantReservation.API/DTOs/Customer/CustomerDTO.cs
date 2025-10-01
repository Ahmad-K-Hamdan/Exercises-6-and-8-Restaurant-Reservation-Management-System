namespace RestaurantReservation.API.DTOs.Customer
{
    public record CustomerDTO(
        int CustomerId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber
    );
}

