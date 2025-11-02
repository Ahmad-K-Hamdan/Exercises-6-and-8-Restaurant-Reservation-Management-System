using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Mappings
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>();
            CreateMap<CreateRestaurantDTO, Restaurant>();
            CreateMap<UpdateRestaurantDTO, Restaurant>();
        }
    }
}