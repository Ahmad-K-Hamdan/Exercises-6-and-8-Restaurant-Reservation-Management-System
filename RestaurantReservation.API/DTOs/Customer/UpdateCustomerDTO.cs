namespace RestaurantReservation.API.DTOs.Customer
{
    public record UpdateCustomerDTO(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber
    );
}

