namespace RestaurantReservation.Db.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public required int Quantity { get; set; }

        public Order? Order { get; set; }
        public MenuItem? MenuItem { get; set; }

        public override string ToString()
        {
            return $"OrderItem [ID: {OrderItemId} | OrderID: {OrderId} | ItemID: {ItemId} | Quantity: {Quantity}]";
        }
    }
}
