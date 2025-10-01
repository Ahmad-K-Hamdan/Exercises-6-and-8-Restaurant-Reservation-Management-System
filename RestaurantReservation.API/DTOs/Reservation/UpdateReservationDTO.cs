namespace RestaurantReservation.API.DTOs.Reservation
{
    public record UpdateReservationDTO(
        int CustomerId,
        int RestaurantId,
        int TableId,
        DateTime ReservationDate,
        int PartySize
    );
}