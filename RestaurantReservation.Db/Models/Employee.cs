namespace RestaurantReservation.Db.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int RestaurantId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Position { get; set; }

        public Restaurant? Restaurant { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        public override string ToString()
        {
            return $"Employee [ID: {EmployeeId} | Name: {FirstName} {LastName} | Position: {Position} | RestaurantID: {RestaurantId}]";
        }
    }
}
