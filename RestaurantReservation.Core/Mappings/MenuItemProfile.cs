using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;

namespace RestaurantReservation.Core.Mappings
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDTO>();
            CreateMap<CreateMenuItemDTO, MenuItem>();
            CreateMap<UpdateMenuItemDTO, MenuItem>();
        }
    }
}