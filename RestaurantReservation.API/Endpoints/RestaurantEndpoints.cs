using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.Restaurant;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.API.Endpoints
{
    public static class RestaurantEndpoints
    {
        public static void MapRestaurantEndpoints(this WebApplication app)
        {
            app.MapGet("/api/restaurants", async ([FromServices] IRestaurantService restaurantService) =>
            {
                var restaurants = await restaurantService.ViewAllAsync();
                var restaurantDTOs = restaurants.Select(ToDTO).ToList();
                return Results.Ok(restaurantDTOs);
            })
            .WithName("GetAllRestaurants")
            .WithSummary("Retrieves all restaurants")
            .WithTags("Restaurant")
            .Produces<IEnumerable<RestaurantDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/restaurants/{id:int}", async (int id, [FromServices] IRestaurantService restaurantService) =>
            {
                var restaurant = await restaurantService.GetRestaurantByIdAsync(id);
                if (restaurant == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(ToDTO(restaurant));
            })
            .WithName("GetRestaurantById")
            .WithSummary("Retrieves a restaurant by its ID")
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/restaurants", async ([FromBody] CreateRestaurantDTO dto, [FromServices] IRestaurantService restaurantService) =>
            {
                var restaurant = await restaurantService.AddAsync(dto.Name, dto.Address, dto.PhoneNumber, dto.OpeningHours);
                return Results.Created($"/api/restaurants/{restaurant.RestaurantId}", ToDTO(restaurant));
            })
            .WithName("AddRestaurant")
            .WithSummary("Creates a new restaurant")
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(201)
            .Produces(400)
            .RequireAuthorization();

            app.MapPut("/api/restaurants/{id:int}", async (int id, [FromBody] UpdateRestaurantDTO dto, [FromServices] IRestaurantService restaurantService) =>
            {
                var existing = await restaurantService.GetRestaurantByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                var restaurant = await restaurantService.UpdateAsync(id, dto.Name, dto.Address, dto.PhoneNumber, dto.OpeningHours);
                return Results.Ok(ToDTO(restaurant));
            })
            .WithName("UpdateRestaurant")
            .WithSummary("Updates an existing restaurant")
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/restaurants/{id:int}", async (int id, [FromServices] IRestaurantService restaurantService) =>
            {
                var existing = await restaurantService.GetRestaurantByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                await restaurantService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteRestaurant")
            .WithSummary("Deletes a restaurant by its ID")
            .WithTags("Restaurant")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();
        }

        private static RestaurantDTO ToDTO(Restaurant restaurant)
        {
            return new RestaurantDTO(
                restaurant.RestaurantId,
                restaurant.Name,
                restaurant.Address,
                restaurant.PhoneNumber,
                restaurant.OpeningHours
            );
        }
    }
}