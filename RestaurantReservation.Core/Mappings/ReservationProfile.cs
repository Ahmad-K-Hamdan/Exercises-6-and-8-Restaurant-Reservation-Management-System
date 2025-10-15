using AutoMapper;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Reservation;

namespace RestaurantReservation.Core.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationDTO>();
            CreateMap<CreateReservationDTO, Reservation>();
            CreateMap<UpdateReservationDTO, Reservation>();
        }
    }
}