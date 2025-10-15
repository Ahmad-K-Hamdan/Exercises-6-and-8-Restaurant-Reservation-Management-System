using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Table;

namespace RestaurantReservation.API.Endpoints
{
    public static class TableEndpoints
    {
        public static void MapTableEndpoints(this WebApplication app)
        {
            app.MapGet("/api/tables", async ([FromServices] ITableService tableService) =>
            {
                return Results.Ok(await tableService.ViewAllAsync());
            })
            .WithName("GetAllTables")
            .WithSummary("Retrieves all tables")
            .WithDescription("""
                Retrieves all tables in the system.

                ### Responses
                - **200 OK**: Returns a list of tables.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Table")
            .Produces<IEnumerable<TableDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/tables/{id:int}", async (int id, [FromServices] ITableService tableService) =>
            {
                return Results.Ok(await tableService.GetTableByIdAsync(id));
            })
            .WithName("GetTableById")
            .WithSummary("Retrieves a table by its ID")
            .WithDescription("""
                Retrieves a specific table by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the table.

                ### Responses
                - **200 OK**: Returns the table details.
                - **404 Not Found**: If the table does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Table")
            .Produces<TableDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/tables", async ([FromBody] CreateTableDTO dto, [FromServices] ITableService tableService) =>
            {
                var table = await tableService.AddAsync(dto);
                return Results.Created($"/api/tables/{table.TableId}", table);

            })
            .WithName("AddTable")
            .WithSummary("Creates a new table")
            .WithDescription("""
                Creates a new table.

                ### Request Body
                - **CreateTableDTO** (required): Object containing table details including Capacity and RestaurantId.

                ### Responses
                - **201 Created**: Returns the newly created table.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If related entities (e.g., Restaurant) do not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Table")
            .Produces<TableDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/tables/{id:int}", async (int id, [FromBody] UpdateTableDTO dto, [FromServices] ITableService tableService) =>
            {
                return Results.Ok(await tableService.UpdateAsync(id, dto));
            })
            .WithName("UpdateTable")
            .WithSummary("Updates an existing table")
            .WithDescription("""
                Updates an existing table by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the table to update.

                ### Request Body
                - **UpdateTableDTO** (required): Object containing updated table details.

                ### Responses
                - **200 OK**: Returns the updated table.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the table does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Table")
            .Produces<TableDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/tables/{id:int}", async (int id, [FromServices] ITableService tableService) =>
            {
                await tableService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteTable")
            .WithSummary("Deletes a table by its ID")
            .WithDescription("""
                Deletes a table by its ID.

                ### Path Parameters
                - **id** (int, required): The ID of the table to delete.

                ### Responses
                - **204 No Content**: If deletion is successful.
                - **404 Not Found**: If the table does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Table")
            .Produces(204)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();
        }
    }
}