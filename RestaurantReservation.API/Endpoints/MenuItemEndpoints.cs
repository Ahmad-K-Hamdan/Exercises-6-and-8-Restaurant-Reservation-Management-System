using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.MenuItem;

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
                    return Results.NotFound();
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
                var menuItem = await menuItemService.AddAsync(dto);
                return Results.Created($"/api/menuitems/{menuItem.ItemId}", ToDTO(menuItem));
            })
            .WithName("AddMenuItem")
            .WithSummary("Creates a new menu item")
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(201)
            .Produces(400)
            .RequireAuthorization();

            app.MapPut("/api/menuitems/{id:int}", async (int id, [FromBody] UpdateMenuItemDTO dto, [FromServices] IMenuItemService menuItemService) =>
            {
                var existing = await menuItemService.GetMenuItemByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                var menuItem = await menuItemService.UpdateAsync(id, dto);
                return Results.Ok(ToDTO(menuItem));
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
                var existing = await menuItemService.GetMenuItemByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                await menuItemService.DeleteAsync(id);
                return Results.NoContent();
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