namespace RestaurantReservation.Db.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public override string ToString()
        {
            return $"Customer [ID: {CustomerId} | Name: {FirstName} {LastName} | Email: {Email} | Phone: {PhoneNumber}]";
        }
    }
}
