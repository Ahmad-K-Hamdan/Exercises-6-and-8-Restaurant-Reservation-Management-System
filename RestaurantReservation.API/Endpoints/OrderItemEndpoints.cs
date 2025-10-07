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
            .WithDescription("""
                Retrieves all order items in the system.

                ### Responses
                - **200 OK**: Returns a list of all order items.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("OrderItem")
            .Produces<IEnumerable<OrderItemDTO>>(200)
            .Produces(401)
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
            .WithDescription("""
                Retrieves a specific order item by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order item to retrieve.

                ### Responses
                - **200 OK**: Returns the order item.
                - **404 Not Found**: If the order item with the specified ID does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Creates a new order item.

                ### Request Body
                - **CreateOrderItemDTO** (required): Object containing order item details, including OrderId, ItemId, and Quantity.

                ### Responses
                - **201 Created**: Returns the newly created order item.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If a referenced order or menu item does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Updates an existing order item by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order item to update.

                ### Request Body
                - **UpdateOrderItemDTO** (required): Object containing updated order item details.

                ### Responses
                - **200 OK**: Returns the updated order item.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the order item does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
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
            .WithDescription("""
                Deletes an order item by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the order item to delete.

                ### Responses
                - **204 No Content**: If deletion is successful.
                - **404 Not Found**: If the order item does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("OrderItem")
            .Produces(204)
            .Produces(404)
            .Produces(401)
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