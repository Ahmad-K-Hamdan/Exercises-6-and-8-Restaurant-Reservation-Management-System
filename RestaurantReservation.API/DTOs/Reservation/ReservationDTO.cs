namespace RestaurantReservation.API.DTOs.Reservation
{
    public record ReservationDTO(
        int ReservationId,
        DateTime ReservationDate,
        int PartySize,
        int CustomerId,
        string CustomerName,
        int RestaurantId,
        string RestaurantName,
        int TableId
    );
}