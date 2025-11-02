namespace RestaurantReservation.Db.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }

        public Restaurant? Restaurant { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public override string ToString()
        {
            return $"MenuItem [ID: {ItemId} | Name: {Name} | Price: {Price:C} | RestaurantID: {RestaurantId}]";
        }
    }
}
