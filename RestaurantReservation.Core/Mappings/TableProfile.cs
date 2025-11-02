using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.Core.Mappings
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableDTO>();
            CreateMap<CreateTableDTO, Table>();
            CreateMap<UpdateTableDTO, Table>();
        }
    }
}
