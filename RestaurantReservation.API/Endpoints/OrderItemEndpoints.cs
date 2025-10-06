using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.OrderItem;
using System.Text.Json;

namespace RestaurantReservation.API.Endpoints
{
    public static class OrderItemEndpoints
    {
        public static void MapOrderItemEndpoints(this WebApplication app)
        {
            app.MapGet("/api/orderitems", async ([FromServices] IOrderItemService orderItemService) =>
            {
                var orderItems = await orderItemService.ViewAllAsync();
                var orderItemDTOs = orderItems.Select(ToDTO).ToList();
                return Results.Ok(orderItemDTOs);
            })
            .WithName("GetAllOrderItems")
            .WithSummary("Retrieves all order items")
            .WithTags("OrderItem")
            .Produces<IEnumerable<OrderItemDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/orderitems/{id:int}", async (int id, [FromServices] IOrderItemService orderItemService) =>
            {
                var orderItem = await orderItemService.GetOrderItemByIdAsync(id);
                if (orderItem == null)
                {
                    return Results.NotFound(new { error = $"Order item with ID {id} not found." });
                }
                return Results.Ok(ToDTO(orderItem));
            })
            .WithName("GetOrderItemById")
            .WithSummary("Retrieves an order item by its ID")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/orderitems", async ([FromBody] CreateOrderItemDTO dto, [FromServices] IOrderItemService orderItemService) =>
            {
                try
                {
                    var orderItem = await orderItemService.AddAsync(dto);
                    return Results.Created($"/api/orderitems/{orderItem.OrderItemId}", ToDTO(orderItem));
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
            .WithName("AddOrderItem")
            .WithSummary("Creates a new order item")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(201)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapPut("/api/orderitems/{id:int}", async (int id, [FromBody] UpdateOrderItemDTO dto, [FromServices] IOrderItemService orderItemService) =>
            {
                try
                {
                    var orderItem = await orderItemService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(orderItem));
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
            .WithName("UpdateOrderItem")
            .WithSummary("Updates an existing order item")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/orderitems/{id:int}", async (int id, [FromServices] IOrderItemService orderItemService) =>
            {
                try
                {
                    await orderItemService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteOrderItem")
            .WithSummary("Deletes an order item by its ID")
            .WithTags("OrderItem")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();
        }

        private static OrderItemDTO ToDTO(OrderItem orderItem)
        {
            return new OrderItemDTO(
                orderItem.OrderItemId,
                orderItem.OrderId,
                orderItem.ItemId,
                orderItem.Quantity,
                orderItem.MenuItem?.Name ?? ""
            );
        }
    }
}