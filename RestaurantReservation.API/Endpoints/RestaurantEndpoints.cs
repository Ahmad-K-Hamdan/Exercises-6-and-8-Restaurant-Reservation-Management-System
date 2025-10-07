using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;
using System.Text.Json;

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
            .WithDescription("""
                Retrieves all restaurants in the system.

                ### Responses
                - **200 OK**: Returns a list of restaurants.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Restaurant")
            .Produces<IEnumerable<RestaurantDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/restaurants/{id:int}", async (int id, [FromServices] IRestaurantService restaurantService) =>
            {
                var restaurant = await restaurantService.GetRestaurantByIdAsync(id);
                if (restaurant == null)
                {
                    return Results.NotFound(new { error = $"Restaurant with ID {id} not found." });
                }
                return Results.Ok(ToDTO(restaurant));
            })
            .WithName("GetRestaurantById")
            .WithSummary("Retrieves a restaurant by its ID")
            .WithDescription("""
                Retrieves a specific restaurant by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the restaurant.

                ### Responses
                - **200 OK**: Returns the restaurant details.
                - **404 Not Found**: If the restaurant does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/restaurants", async ([FromBody] CreateRestaurantDTO dto, [FromServices] IRestaurantService restaurantService) =>
            {
                try
                {
                    var restaurant = await restaurantService.AddAsync(dto);
                    return Results.Created($"/api/restaurants/{restaurant.RestaurantId}", ToDTO(restaurant));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
            })
            .WithName("AddRestaurant")
            .WithSummary("Creates a new restaurant")
            .WithDescription("""
                Creates a new restaurant.

                ### Request Body
                - **CreateRestaurantDTO** (required): Object containing restaurant details including Name, Address, PhoneNumber, and OpeningHours.

                ### Responses
                - **201 Created**: Returns the newly created restaurant.
                - **400 Bad Request**: If validation fails.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(201)
            .Produces(400)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/restaurants/{id:int}", async (int id, [FromBody] UpdateRestaurantDTO dto, [FromServices] IRestaurantService restaurantService) =>
            {
                try
                {
                    var restaurant = await restaurantService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(restaurant));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("UpdateRestaurant")
            .WithSummary("Updates an existing restaurant")
            .WithDescription("""
                Updates an existing restaurant by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the restaurant to update.

                ### Request Body
                - **UpdateRestaurantDTO** (required): Object containing updated restaurant details.

                ### Responses
                - **200 OK**: Returns the updated restaurant.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the restaurant does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Restaurant")
            .Produces<RestaurantDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/restaurants/{id:int}", async (int id, [FromServices] IRestaurantService restaurantService) =>
            {
                try
                {
                    await restaurantService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteRestaurant")
            .WithSummary("Deletes a restaurant by its ID")
            .WithDescription("""
                Deletes a restaurant by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the restaurant to delete.

                ### Responses
                - **204 No Content**: If deletion is successful.
                - **404 Not Found**: If the restaurant does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Restaurant")
            .Produces(204)
            .Produces(404)
            .Produces(401)
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