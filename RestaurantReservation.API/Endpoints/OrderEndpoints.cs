using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Order;
using System.Text.Json;

namespace RestaurantReservation.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            app.MapGet("/api/orders", async ([FromServices] IOrderService orderService) =>
            {
                var orders = await orderService.ViewAllAsync();
                var orderDTOs = orders.Select(ToDTO).ToList();
                return Results.Ok(orderDTOs);
            })
            .WithName("GetAllOrders")
            .WithSummary("Retrieves all orders")
            .WithTags("Order")
            .Produces<IEnumerable<OrderDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/orders/{id:int}", async (int id, [FromServices] IOrderService orderService) =>
            {
                var order = await orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return Results.NotFound(new { error = $"Order with ID {id} not found." });
                }
                return Results.Ok(ToDTO(order));
            })
            .WithName("GetOrderById")
            .WithSummary("Retrieves an order by its ID")
            .WithTags("Order")
            .Produces<OrderDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/orders", async ([FromBody] CreateOrderDTO dto, [FromServices] IOrderService orderService) =>
            {
                try
                {
                    var order = await orderService.AddAsync(dto);
                    return Results.Created($"/api/orders/{order.OrderId}", ToDTO(order));
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
            .WithName("AddOrder")
            .WithSummary("Creates a new order")
            .WithTags("Order")
            .Produces<OrderDTO>(201)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapPut("/api/orders/{id:int}", async (int id, [FromBody] UpdateOrderDTO dto, [FromServices] IOrderService orderService) =>
            {
                try
                {
                    var order = await orderService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(order));
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
            .WithName("UpdateOrder")
            .WithSummary("Updates an existing order")
            .WithTags("Order")
            .Produces<OrderDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/orders/{id:int}", async (int id, [FromServices] IOrderService orderService) =>
            {
                try
                {
                    await orderService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteOrder")
            .WithSummary("Deletes an order by its ID")
            .WithTags("Order")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();
        }

        private static OrderDTO ToDTO(Order order)
        {
            return new OrderDTO(
                order.OrderId,
                order.OrderDate,
                order.TotalAmount,
                order.ReservationId,
                order.EmployeeId,
                order.Employee != null ? $"{order.Employee.FirstName} {order.Employee.LastName}" : ""
            );
        }
    }
}