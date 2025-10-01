namespace RestaurantReservation.API.DTOs.Customer
{
    public record UpdateCustomerDTO(
        int CustomerId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber
    );
}

