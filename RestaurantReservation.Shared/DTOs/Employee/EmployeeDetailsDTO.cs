namespace RestaurantReservation.Core.DTOs
{
    public class EmployeeDetailsDTO
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        public override string ToString()
        {
            return $"Employee {EmployeeId} | {FirstName} {LastName} | Position: {Position} | Restaurant: {Name} | Address: {Address}";
        }
    }
}