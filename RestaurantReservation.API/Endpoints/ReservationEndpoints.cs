using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;
using RestaurantReservation.Shared.DTOs.Order;
using RestaurantReservation.Shared.DTOs.Reservation;
using System.Text.Json;

namespace RestaurantReservation.API.Endpoints
{
    public static class ReservationEndpoints
    {
        public static void MapReservationEndpoints(this WebApplication app)
        {
            app.MapGet("/api/reservations", async ([FromServices] IReservationService reservationService) =>
            {
                var reservations = await reservationService.ViewAllAsync();
                var reservationDTOs = reservations.Select(ToDTO).ToList();
                return Results.Ok(reservationDTOs);
            })
            .WithName("GetAllReservations")
            .WithSummary("Retrieves all reservations")
            .WithDescription("""
                Retrieves all reservations in the system.

                ### Responses
                - **200 OK**: Returns a list of reservations.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<IEnumerable<ReservationDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/reservations/{id:int}", async (int id, [FromServices] IReservationService reservationService) =>
            {
                var reservation = await reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return Results.NotFound(new { error = $"Reservation with ID {id} not found." });
                }
                return Results.Ok(ToDTO(reservation));
            })
            .WithName("GetReservationById")
            .WithSummary("Retrieves a reservation by its ID")
            .WithDescription("""
                Retrieves a specific reservation by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the reservation to retrieve.

                ### Responses
                - **200 OK**: Returns the reservation details.
                - **404 Not Found**: If the reservation does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<ReservationDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/reservations", async ([FromBody] CreateReservationDTO dto, [FromServices] IReservationService reservationService) =>
            {
                try
                {
                    var reservation = await reservationService.AddAsync(dto);
                    return Results.Created($"/api/reservations/{reservation.ReservationId}", ToDTO(reservation));
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
            .WithName("AddReservation")
            .WithSummary("Creates a new reservation")
            .WithDescription("""
                Creates a new reservation.

                ### Request Body
                - **CreateReservationDTO** (required): Object containing reservation details including CustomerId, ReservationDate, PartySize, RestaurantId, and TableId.

                ### Responses
                - **201 Created**: Returns the newly created reservation.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If a referenced customer, restaurant, or table does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<ReservationDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/reservations/{id:int}", async (int id, [FromBody] UpdateReservationDTO dto, [FromServices] IReservationService reservationService) =>
            {
                try
                {
                    var reservation = await reservationService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(reservation));
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
            .WithName("UpdateReservation")
            .WithSummary("Updates an existing reservation")
            .WithDescription("""
                Updates an existing reservation by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the reservation to update.

                ### Request Body
                - **UpdateReservationDTO** (required): Object containing updated reservation details.

                ### Responses
                - **200 OK**: Returns the updated reservation.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the reservation does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<ReservationDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/reservations/{id:int}", async (int id, [FromServices] IReservationService reservationService) =>
            {
                try
                {
                    await reservationService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteReservation")
            .WithSummary("Deletes a reservation by its ID")
            .WithDescription("""
                Deletes a reservation by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the reservation to delete.

                ### Responses
                - **204 No Content**: If deletion is successful.
                - **404 Not Found**: If the reservation does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces(204)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/reservations/customer/{customerId}", async (int customerId, [FromServices] IReservationService reservationService, [FromServices] ICustomerService customerService) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    return Results.NotFound(new { error = $"Customer with ID {customerId} not found." });
                }
                var reservations = await reservationService.ListReservationsByCustomerAsync(customerId);
                var reservationDTOs = reservations.Select(ToDTO).ToList();
                return Results.Ok(reservationDTOs);
            })
            .WithName("GetReservationsByCustomerId")
            .WithSummary("Retrieves all reservations for a specific customer by customer ID")
            .WithDescription("""
                Retrieves all reservations for a specific customer.

                ### Path Parameters
                - **customerId** (int, required): The ID of the customer.

                ### Responses
                - **200 OK**: Returns a list of reservations for the customer.
                - **404 Not Found**: If the customer does not exist or has no reservations.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<IEnumerable<ReservationDTO>>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/reservations/{reservationId}/orders", async (int reservationId, [FromServices] IReservationService reservationService) =>
            {
                var reservation = await reservationService.GetReservationByIdAsync(reservationId);
                if (reservation == null)
                {
                    return Results.NotFound(new { error = $"Reservation with ID {reservationId} not found." });
                }
                var orders = await reservationService.ListOrdersAndMenuItemsAsync(reservationId);
                return Results.Ok(orders);
            })
            .WithName("GetOrdersByReservationId")
            .WithSummary("Retrieves all orders and their menu items for a specific reservation by reservation ID")
            .WithDescription("""
                Retrieves all orders and their associated menu items for a specific reservation.

                ### Path Parameters
                - **reservationId** (int, required): The ID of the reservation.

                ### Responses
                - **200 OK**: Returns a list of orders with menu items.
                - **404 Not Found**: If the reservation does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<IEnumerable<OrderWithItemsDTO>>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/reservations/{reservationId}/menu-items", async (int reservationId, [FromServices] IReservationService reservationService) =>
            {
                var reservation = await reservationService.GetReservationByIdAsync(reservationId);
                if (reservation == null)
                {
                    return Results.NotFound(new { error = $"Reservation with ID {reservationId} not found." });
                }
                var menuItems = await reservationService.ListOrderedMenuItemsAsync(reservationId);
                return Results.Ok(menuItems);
            })
            .WithName("GetMenuItemsByReservationId")
            .WithSummary("Retrieves all ordered menu items for a specific reservation by reservation ID")
            .WithDescription("""
                Retrieves all menu items ordered for a specific reservation.

                ### Path Parameters
                - **reservationId** (int, required): The ID of the reservation.

                ### Responses
                - **200 OK**: Returns a list of ordered menu items.
                - **404 Not Found**: If the reservation does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Reservation")
            .Produces<IEnumerable<OrderedMenuItemDTO>>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();
        }

        private static ReservationDTO ToDTO(Reservation reservation)
        {
            return new ReservationDTO(
                reservation.ReservationId,
                reservation.ReservationDate,
                reservation.PartySize,
                reservation.CustomerId,
                reservation.Customer != null ? $"{reservation.Customer.FirstName} {reservation.Customer.LastName}" : "",
                reservation.RestaurantId,
                reservation.Restaurant?.Name ?? "",
                reservation.TableId
            );
        }
    }
}