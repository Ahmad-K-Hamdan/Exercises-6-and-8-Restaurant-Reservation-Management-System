namespace RestaurantReservation.Shared.DTOs.Customer
{
    public record CreateCustomerDTO(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber
    );
}

