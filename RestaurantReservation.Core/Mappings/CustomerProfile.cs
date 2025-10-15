using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.Core.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CreateCustomerDTO, Customer>();
            CreateMap<UpdateCustomerDTO, Customer>();
        }
    }
}