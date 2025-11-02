namespace RestaurantReservation.Db.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int TableId { get; set; }
        public required DateTime ReservationDate { get; set; }
        public required int PartySize { get; set; }

        public Customer? Customer { get; set; }
        public Restaurant? Restaurant { get; set; }
        public Table? Table { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public override string ToString()
        {
            return $"Reservation [ID: {ReservationId} | Date: {ReservationDate:g} | PartySize: {PartySize} | CustomerID: {CustomerId} | TableID: {TableId} | RestaurantID: {RestaurantId}]";
        }
    }
}
