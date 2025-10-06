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
            .WithTags("Reservation")
            .Produces<IEnumerable<ReservationDTO>>(200)
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
            .WithTags("Reservation")
            .Produces<ReservationDTO>(200)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces<ReservationDTO>(201)
            .Produces(400)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces<ReservationDTO>(200)
            .Produces(400)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces(204)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces<IEnumerable<ReservationDTO>>(200)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces<IEnumerable<OrderWithItemsDTO>>(200)
            .Produces(404)
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
            .WithTags("Reservation")
            .Produces<IEnumerable<OrderedMenuItemDTO>>(200)
            .Produces(404)
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