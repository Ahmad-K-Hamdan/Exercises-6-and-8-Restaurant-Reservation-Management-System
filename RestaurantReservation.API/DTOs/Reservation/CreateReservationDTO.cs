namespace RestaurantReservation.API.DTOs.Reservation
{
    public record CreateReservationDTO(
        int CustomerId,
        int RestaurantId,
        int TableId,
        DateTime ReservationDate,
        int PartySize
    );
}