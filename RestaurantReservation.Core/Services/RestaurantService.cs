using AutoMapper;
using FluentValidation;
using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IValidator<CreateRestaurantDTO> _createValidator;
        private readonly IValidator<UpdateRestaurantDTO> _updateValidator;
        private readonly IMapper _mapper;

        public RestaurantService(
            IRestaurantRepository restaurantRepo,
            IValidator<CreateRestaurantDTO> createValidator,
            IValidator<UpdateRestaurantDTO> updateValidator,
            IMapper mapper)
        {
            _restaurantRepo = restaurantRepo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDTO>> ViewAllAsync()
        {
            var restaurants = await _restaurantRepo.GetAllAsync();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public async Task<RestaurantDTO> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            return _mapper.Map<RestaurantDTO>(restaurant);
        }

        public async Task<RestaurantDTO> AddAsync(CreateRestaurantDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var newRestaurant = _mapper.Map<Restaurant>(dto);
            var restaurant = await _restaurantRepo.AddAsync(newRestaurant);

            return _mapper.Map<RestaurantDTO>(restaurant);
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            await _restaurantRepo.DeleteAsync(restaurant);
        }

        public async Task<RestaurantDTO> UpdateAsync(int restaurantId, UpdateRestaurantDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }

            _mapper.Map(dto, restaurant);

            var updatedRestaurant = await _restaurantRepo.UpdateAsync(restaurant);
            return _mapper.Map<RestaurantDTO>(updatedRestaurant);
        }

        public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            return await _restaurantRepo.GetRestaurantRevenueAsync(restaurantId);
        }
    }
}