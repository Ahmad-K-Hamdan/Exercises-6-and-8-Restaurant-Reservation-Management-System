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
            .WithDescription("""
                Retrieves all orders in the system.

                ### Responses
                - **200 OK**: Returns a list of all orders.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Order")
            .Produces<IEnumerable<OrderDTO>>(200)
            .Produces(401)
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
            .WithDescription("""
                Retrieves a specific order by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order to retrieve.

                ### Responses
                - **200 OK**: Returns the order.
                - **404 Not Found**: If the order with the specified ID does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Order")
            .Produces<OrderDTO>(200)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Creates a new order.

                ### Request Body
                - **CreateOrderDTO** (required): Object containing order details, including ReservationId, EmployeeId, and TotalAmount.

                ### Responses
                - **201 Created**: Returns the newly created order.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If a referenced reservation or employee does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Order")
            .Produces<OrderDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Updates an existing order by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order to update.

                ### Request Body
                - **UpdateOrderDTO** (required): Object containing updated order details.

                ### Responses
                - **200 OK**: Returns the updated order.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the order does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Order")
            .Produces<OrderDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Deletes an order by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order to delete.

                ### Responses
                - **204 No Content**: If deletion is successful.
                - **404 Not Found**: If the order does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Order")
            .Produces(204)
            .Produces(404)
            .Produces(401)
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