using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;
using System.Text.Json;

namespace RestaurantReservation.API.Endpoints
{
    public static class MenuItemEndpoints
    {
        public static void MapMenuItemEndpoints(this WebApplication app)
        {
            app.MapGet("/api/menuitems", async ([FromServices] IMenuItemService menuItemService) =>
            {
                var menuItems = await menuItemService.ViewAllAsync();
                var menuItemDTOs = menuItems.Select(ToDTO).ToList();
                return Results.Ok(menuItemDTOs);
            })
            .WithName("GetAllMenuItems")
            .WithSummary("Retrieves all menu items")
            .WithTags("MenuItem")
            .Produces<IEnumerable<MenuItemDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/menuitems/{id:int}", async (int id, [FromServices] IMenuItemService menuItemService) =>
            {
                var menuItem = await menuItemService.GetMenuItemByIdAsync(id);
                if (menuItem == null)
                {
                    return Results.NotFound(new { error = $"Menu item with ID {id} not found." });
                }
                return Results.Ok(ToDTO(menuItem));
            })
            .WithName("GetMenuItemById")
            .WithSummary("Retrieves a menu item by its ID")
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/menuitems", async ([FromBody] CreateMenuItemDTO dto, [FromServices] IMenuItemService menuItemService) =>
            {
                try
                {
                    var menuItem = await menuItemService.AddAsync(dto);
                    return Results.Created($"/api/menuitems/{menuItem.ItemId}", ToDTO(menuItem));
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
            .WithName("AddMenuItem")
            .WithSummary("Creates a new menu item")
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(201)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapPut("/api/menuitems/{id:int}", async (int id, [FromBody] UpdateMenuItemDTO dto, [FromServices] IMenuItemService menuItemService) =>
            {
                try
                {
                    var menuItem = await menuItemService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(menuItem));
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
            .WithName("UpdateMenuItem")
            .WithSummary("Updates an existing menu item")
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/menuitems/{id:int}", async (int id, [FromServices] IMenuItemService menuItemService) =>
            {
                try
                {
                    await menuItemService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteMenuItem")
            .WithSummary("Deletes a menu item by its ID")
            .WithTags("MenuItem")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();
        }

        private static MenuItemDTO ToDTO(MenuItem menuItem)
        {
            return new MenuItemDTO(
                menuItem.ItemId,
                menuItem.Name,
                menuItem.Description,
                menuItem.Price,
                menuItem.RestaurantId,
                menuItem.Restaurant?.Name ?? ""
            );
        }
    }
}