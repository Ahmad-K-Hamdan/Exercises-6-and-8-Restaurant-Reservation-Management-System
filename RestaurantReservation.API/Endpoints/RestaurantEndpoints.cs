using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Restaurant;

namespace RestaurantReservation.API.Endpoints
{
    public static class RestaurantEndpoints
    {
        public static void MapRestaurantEndpoints(this WebApplication app)
        {
            app.MapGet("/api/restaurants", async ([FromServices] IRestaurantService restaurantService) =>
            {
                return Results.Ok(await restaurantService.ViewAllAsync());
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
                return Results.Ok(await restaurantService.GetRestaurantByIdAsync(id));
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
                var restaurant = await restaurantService.AddAsync(dto);
                return Results.Created($"/api/restaurants/{restaurant.RestaurantId}", restaurant);
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
                return Results.Ok(await restaurantService.UpdateAsync(id, dto));
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
                await restaurantService.DeleteAsync(id);
                return Results.NoContent();
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
    }
}