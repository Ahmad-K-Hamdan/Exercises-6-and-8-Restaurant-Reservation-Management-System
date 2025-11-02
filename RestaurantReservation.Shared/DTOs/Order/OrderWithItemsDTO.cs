using RestaurantReservation.Shared.DTOs.MenuItem;

namespace RestaurantReservation.Shared.DTOs.Order
{
    public class OrderWithItemsDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ReservationId { get; set; }
        public List<OrderedMenuItemDTO> Items { get; set; } = new List<OrderedMenuItemDTO>();

        public override string ToString()
        {
            return $"Order {OrderId} | Date: {OrderDate:g} | Total: {TotalAmount:C} | Items: {Items.Count}";
        }
    }
}