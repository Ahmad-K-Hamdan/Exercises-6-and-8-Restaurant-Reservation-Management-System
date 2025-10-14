using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.MenuItem;

namespace RestaurantReservation.API.Endpoints
{
    public static class MenuItemEndpoints
    {
        public static void MapMenuItemEndpoints(this WebApplication app)
        {
            app.MapGet("/api/menuitems", async ([FromServices] IMenuItemService menuItemService) =>
            {
                return Results.Ok(await menuItemService.ViewAllAsync());
            })
            .WithName("GetAllMenuItems")
            .WithSummary("Retrieves all menu items")
            .WithDescription("""
                Retrieves all menu items available in the system.

                ### Responses
                - **200 OK**: Returns a list of all menu items.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("MenuItem")
            .Produces<IEnumerable<MenuItemDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/menuitems/{id:int}", async (int id, [FromServices] IMenuItemService menuItemService) =>
            {
                return Results.Ok(await menuItemService.GetMenuItemByIdAsync(id));
            })
            .WithName("GetMenuItemById")
            .WithSummary("Retrieves a menu item by its ID")
            .WithDescription("""
                Retrieves a single menu item by its unique identifier.

                ### Path Parameters
                - **id** (int, required): The ID of the menu item to retrieve.

                ### Responses
                - **200 OK**: Returns the menu item details.
                - **404 Not Found**: If no menu item exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/menuitems", async ([FromBody] CreateMenuItemDTO dto, [FromServices] IMenuItemService menuItemService) =>
            {
                var menuItem = await menuItemService.AddAsync(dto);
                return Results.Created($"/api/menuitems/{menuItem.ItemId}", menuItem);
            })
            .WithName("AddMenuItem")
            .WithSummary("Creates a new menu item")
            .WithDescription("""
                Creates a new menu item for a specific restaurant.

                ### Request Body
                - **CreateMenuItemDTO** (required): Menu item data including Name, Description, Price, and RestaurantId.

                ### Responses
                - **201 Created**: Returns the created menu item details.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the referenced restaurant does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/menuitems/{id:int}", async (int id, [FromBody] UpdateMenuItemDTO dto, [FromServices] IMenuItemService menuItemService) =>
            {
                return Results.Ok(await menuItemService.UpdateAsync(id, dto));
            })
            .WithName("UpdateMenuItem")
            .WithSummary("Updates an existing menu item")
            .WithDescription("""
                Updates an existing menu item's information.

                ### Path Parameters
                - **id** (int, required): The ID of the menu item to update.

                ### Request Body
                - **UpdateMenuItemDTO** (required): Updated menu item data including Name, Description, and Price.

                ### Responses
                - **200 OK**: Returns the updated menu item details.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If no menu item exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("MenuItem")
            .Produces<MenuItemDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/menuitems/{id:int}", async (int id, [FromServices] IMenuItemService menuItemService) =>
            {
                await menuItemService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteMenuItem")
            .WithSummary("Deletes a menu item by its ID")
            .WithDescription("""
                Deletes a menu item from the system.

                ### Path Parameters
                - **id** (int, required): The ID of the menu item to delete.

                ### Responses
                - **204 No Content**: Menu item successfully deleted.
                - **404 Not Found**: If no menu item exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("MenuItem")
            .Produces(204)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();
        }
    }
}