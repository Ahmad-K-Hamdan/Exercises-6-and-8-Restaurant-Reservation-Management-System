namespace RestaurantReservation.Core.DTOs
{
    public class ReservationDetailsDTO
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public override string ToString()
        {
            return $"Reservation {ReservationId} | Date: {ReservationDate:g} | Party: {PartySize} | Customer: {FirstName} {LastName} | Restaurant: {Name} | Address: {Address}";
        }
    }
}