using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.OrderItem;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Services.Interfaces;

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
            .Produces<IEnumerable<OrderItemDTO>>(200);

            app.MapGet("/api/orderitems/{id:int}", async (int id, [FromServices] IOrderItemService orderItemService) =>
            {
                var orderItem = await orderItemService.GetOrderItemByIdAsync(id);
                if (orderItem == null)
                    return Results.NotFound();

                return Results.Ok(ToDTO(orderItem));
            })
            .WithName("GetOrderItemById")
            .WithSummary("Retrieves an order item by its ID")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(404);

            app.MapPost("/api/orderitems", async ([FromBody] CreateOrderItemDTO dto, [FromServices] IOrderItemService orderItemService) =>
            {
                var orderItem = await orderItemService.AddAsync(dto.OrderId, dto.ItemId, dto.Quantity);
                return Results.Created($"/api/orderitems/{orderItem.OrderItemId}", ToDTO(orderItem));
            })
            .WithName("AddOrderItem")
            .WithSummary("Creates a new order item")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(201)
            .Produces(400);

            app.MapPut("/api/orderitems/{id:int}", async (int id, [FromBody] UpdateOrderItemDTO dto, [FromServices] IOrderItemService orderItemService) =>
            {
                if (id != dto.OrderItemId)
                    return Results.BadRequest("IDs do not match");

                var existing = await orderItemService.GetOrderItemByIdAsync(id);
                if (existing == null)
                    return Results.NotFound();

                var orderItem = await orderItemService.UpdateAsync(dto.OrderItemId, dto.OrderId, dto.ItemId, dto.Quantity);
                return Results.Ok(ToDTO(orderItem));
            })
            .WithName("UpdateOrderItem")
            .WithSummary("Updates an existing order item")
            .WithTags("OrderItem")
            .Produces<OrderItemDTO>(200)
            .Produces(400)
            .Produces(404);

            app.MapDelete("/api/orderitems/{id:int}", async (int id, [FromServices] IOrderItemService orderItemService) =>
            {
                var existing = await orderItemService.GetOrderItemByIdAsync(id);
                if (existing == null)
                    return Results.NotFound();

                await orderItemService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteOrderItem")
            .WithSummary("Deletes an order item by its ID")
            .WithTags("OrderItem")
            .Produces(204)
            .Produces(404);
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