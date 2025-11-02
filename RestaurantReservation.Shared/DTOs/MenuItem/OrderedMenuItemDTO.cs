namespace RestaurantReservation.Shared.DTOs.MenuItem
{
    public class OrderedMenuItemDTO
    {
        public string? MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{MenuItemName} (Qty: {Quantity}) - {Price:C}";
        }
    }
}