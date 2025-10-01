namespace RestaurantReservation.Db.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public int RestaurantId { get; set; }
        public required int Capacity { get; set; }

        public required Restaurant Restaurant { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public override string ToString()
        {
            return $"Table [ID: {TableId} | Capacity: {Capacity} | RestaurantID: {RestaurantId}]";
        }
    }
}
