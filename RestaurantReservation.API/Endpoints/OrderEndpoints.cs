using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.Order;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Services.Interfaces;

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
                    return Results.NotFound();
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
                var order = await orderService.AddAsync(dto.ReservationId, dto.EmployeeId, dto.OrderDate, dto.TotalAmount);
                return Results.Created($"/api/orders/{order.OrderId}", ToDTO(order));
            })
            .WithName("AddOrder")
            .WithSummary("Creates a new order")
            .WithTags("Order")
            .Produces<OrderDTO>(201)
            .Produces(400)
            .RequireAuthorization();

            app.MapPut("/api/orders/{id:int}", async (int id, [FromBody] UpdateOrderDTO dto, [FromServices] IOrderService orderService) =>
            {
                var existing = await orderService.GetOrderByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                var order = await orderService.UpdateAsync(id, dto.ReservationId, dto.EmployeeId, dto.OrderDate, dto.TotalAmount);
                return Results.Ok(ToDTO(order));
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
                var existing = await orderService.GetOrderByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                await orderService.DeleteAsync(id);
                return Results.NoContent();
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