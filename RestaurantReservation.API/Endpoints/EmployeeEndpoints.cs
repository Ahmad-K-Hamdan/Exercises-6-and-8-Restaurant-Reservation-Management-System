using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/api/employees", async ([FromServices] IEmployeeService employeeService) =>
            {
                var employees = await employeeService.ViewAllAsync();
                var employeeDTOs = employees.Select(ToDTO).ToList();
                return Results.Ok(employeeDTOs);
            })
            .WithName("GetAllEmployees")
            .WithSummary("Retrieves all employees")
            .WithTags("Employee")
            .Produces<IEnumerable<EmployeeDTO>>(200)
            .RequireAuthorization();

            app.MapGet("/api/employees/{id:int}", async (int id, [FromServices] IEmployeeService employeeService) =>
            {
                var employee = await employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return Results.NotFound(new { error = $"Employee with ID {id} not found." });
                }
                return Results.Ok(ToDTO(employee));
            })
            .WithName("GetEmployeeById")
            .WithSummary("Retrieves an employee by its ID")
            .WithTags("Employee")
            .Produces<EmployeeDTO>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapPost("/api/employees", async ([FromBody] CreateEmployeeDTO dto, [FromServices] IEmployeeService employeeService) =>
            {
                try
                {
                    var employee = await employeeService.AddAsync(dto);
                    return Results.Created($"/api/employees/{employee.EmployeeId}", ToDTO(employee));
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
            .WithName("AddEmployee")
            .WithSummary("Creates a new employee")
            .WithTags("Employee")
            .Produces<EmployeeDTO>(201)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapPut("/api/employees/{id:int}", async (int id, [FromBody] UpdateEmployeeDTO dto, [FromServices] IEmployeeService employeeService) =>
            {
                try
                {
                    var employee = await employeeService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(employee));
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
            .WithName("UpdateEmployee")
            .WithSummary("Updates an existing employee")
            .WithTags("Employee")
            .Produces<EmployeeDTO>(200)
            .Produces(400)
            .Produces(404)
            .RequireAuthorization();

            app.MapDelete("/api/employees/{id:int}", async (int id, [FromServices] IEmployeeService employeeService) =>
            {
                try
                {
                    await employeeService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteEmployee")
            .WithSummary("Deletes an employee by its ID")
            .WithTags("Employee")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();

            app.MapGet("/api/employees/managers", async ([FromServices] IEmployeeService employeeService) =>
            {
                var managers = await employeeService.ListManagersAsync();
                if (managers == null || !managers.Any())
                {
                    return Results.NotFound(new { error = "No managers found." });
                }
                var managerDTOs = managers.Select(ToDTO).ToList();
                return Results.Ok(managerDTOs);
            })
            .WithName("GetAllManagers")
            .WithSummary("Retrieves all employees with the position of 'Manager'")
            .WithTags("Employee")
            .Produces<IEnumerable<EmployeeDTO>>(200)
            .Produces(404)
            .RequireAuthorization();

            app.MapGet("/api/employees/{employeeId}/average-order-amount", async (int employeeId, [FromServices] IEmployeeService employeeService, [FromServices] IOrderService orderService) =>
            {
                var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                {
                    return Results.NotFound(new { error = $"Employee with ID {employeeId} not found." });
                }
                var average = await orderService.CalculateAverageOrderAmountByEmployeeAsync(employeeId);
                return Results.Ok(new { employeeId, averageOrderAmount = average });
            })
            .WithName("GetAverageOrderAmountByEmployee")
            .WithSummary("Calculate average order amount for a specific employee")
            .WithTags("Employee")
            .Produces<object>(200)
            .Produces(404)
            .RequireAuthorization();
        }

        private static EmployeeDTO ToDTO(Employee employee)
        {
            return new EmployeeDTO(
                employee.EmployeeId,
                employee.FirstName,
                employee.LastName,
                employee.Position,
                employee.RestaurantId,
                employee.Restaurant?.Name ?? ""
            );
        }
    }
}