namespace RestaurantReservation.Core.DTOs
{
    public class CustomerDetailsDTO
    {
        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }

        public override string ToString()
        {
            return $"Customer {CustomerId} | {FirstName} {LastName} | Reservation Date: {ReservationDate} | Party Size: {PartySize}";
        }
    }
}
