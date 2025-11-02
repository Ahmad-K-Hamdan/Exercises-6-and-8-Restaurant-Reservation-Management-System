using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.OrderItem;

namespace RestaurantReservation.Core.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<CreateOrderItemDTO, OrderItem>();
            CreateMap<UpdateOrderItemDTO, OrderItem>();
        }
    }
}