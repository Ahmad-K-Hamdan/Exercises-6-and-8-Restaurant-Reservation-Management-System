namespace RestaurantReservation.Db.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required TimeSpan OpeningHours { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public override string ToString()
        {
            return $"Restaurant [ID: {RestaurantId} | Name: {Name} | Address: {Address} | Phone: {PhoneNumber} | OpeningHours: {OpeningHours}]";
        }
    }
}
