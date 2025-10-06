using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Table;
using System.Text.Json;

namespace RestaurantReservation.API.Endpoints
{
    public static class TableEndpoints
    {
        public static void MapTableEndpoints(this WebApplication app)
        {
            app.MapGet("/api/tables", async ([FromServices] ITableService tableService) =>
            {
                var tables = await tableService.ViewAllAsync();
                var tableDTOs = tables.Select(ToDTO).ToList();
                return Results.Ok(tableDTOs);
            })
            .WithName("GetAllTables")
            .WithSummary("Retrieves all tables")
            .WithTags("Table")
            .Produces<IEnumerable<TableDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/tables/{id:int}", async (int id, [FromServices] ITableService tableService) =>
            {
                var table = await tableService.GetTableByIdAsync(id);
                if (table == null)
                {
                    return Results.NotFound(new { error = $"Table with ID {id} not found." });
                }
                return Results.Ok(ToDTO(table));
            })
            .WithName("GetTableById")
            .WithSummary("Retrieves a table by its ID")
            .WithTags("Table")
            .Produces<TableDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/tables", async ([FromBody] CreateTableDTO dto, [FromServices] ITableService tableService) =>
            {
                try
                {
                    var table = await tableService.AddAsync(dto);
                    return Results.Created($"/api/tables/{table.TableId}", ToDTO(table));
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
            .WithName("AddTable")
            .WithSummary("Creates a new table")
            .WithTags("Table")
            .Produces<TableDTO>(201)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapPut("/api/tables/{id:int}", async (int id, [FromBody] UpdateTableDTO dto, [FromServices] ITableService tableService) =>
            {
                try
                {
                    var table = await tableService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(table));
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
            .WithName("UpdateTable")
            .WithSummary("Updates an existing table")
            .WithTags("Table")
            .Produces<TableDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/tables/{id:int}", async (int id, [FromServices] ITableService tableService) =>
            {
                try
                {
                    await tableService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteTable")
            .WithSummary("Deletes a table by its ID")
            .WithTags("Table")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();
        }

        private static TableDTO ToDTO(Table table)
        {
            return new TableDTO(
                table.TableId,
                table.Capacity,
                table.RestaurantId,
                table.Restaurant?.Name ?? ""
            );
        }
    }
}