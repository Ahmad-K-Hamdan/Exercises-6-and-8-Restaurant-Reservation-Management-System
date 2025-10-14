using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/api/employees", async ([FromServices] IEmployeeService employeeService) =>
            {
                return Results.Ok(await employeeService.ViewAllAsync());
            })
            .WithName("GetAllEmployees")
            .WithSummary("Retrieves all employees")
            .WithDescription("""
                Retrieves all employees in the system.

                ### Responses
                - **200 OK**: Returns a list of all employees.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<IEnumerable<EmployeeDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/employees/{id:int}", async (int id, [FromServices] IEmployeeService employeeService) =>
            {
                return Results.Ok(await employeeService.GetEmployeeByIdAsync(id));
            })
            .WithName("GetEmployeeById")
            .WithSummary("Retrieves an employee by its ID")
            .WithDescription("""
                Retrieves a single employee by their unique identifier.

                ### Path Parameters
                - **id** (int, required): The ID of the employee to retrieve.

                ### Responses
                - **200 OK**: Returns the employee details.
                - **404 Not Found**: If no employee exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<EmployeeDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/employees", async ([FromBody] CreateEmployeeDTO dto, [FromServices] IEmployeeService employeeService) =>
            {
                var employee = await employeeService.AddAsync(dto);
                return Results.Created($"/api/employees/{employee.EmployeeId}", employee);
            })
            .WithName("AddEmployee")
            .WithSummary("Creates a new employee")
            .WithDescription("""
                Creates a new employee for a specified restaurant.

                ### Request Body
                - **CreateEmployeeDTO** (required): Employee data including FirstName, LastName, Position, and RestaurantId.

                ### Responses
                - **201 Created**: Returns the created employee details.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the referenced restaurant does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<EmployeeDTO>(201)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/employees/{id:int}", async (int id, [FromBody] UpdateEmployeeDTO dto, [FromServices] IEmployeeService employeeService) =>
            {
                return Results.Ok(await employeeService.UpdateAsync(id, dto));
            })
            .WithName("UpdateEmployee")
            .WithSummary("Updates an existing employee")
            .WithDescription("""
                Updates an existing employee's information.

                ### Path Parameters
                - **id** (int, required): The ID of the employee to update.

                ### Request Body
                - **UpdateEmployeeDTO** (required): Updated employee data including FirstName, LastName, and Position.

                ### Responses
                - **200 OK**: Returns the updated employee details.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If no employee exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<EmployeeDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/employees/{id:int}", async (int id, [FromServices] IEmployeeService employeeService) =>
            {
                await employeeService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteEmployee")
            .WithSummary("Deletes an employee by its ID")
            .WithDescription("""
                Deletes an employee from the system.

                ### Path Parameters
                - **id** (int, required): The ID of the employee to delete.

                ### Responses
                - **204 No Content**: Employee successfully deleted.
                - **404 Not Found**: If no employee exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces(204)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/employees/managers", async ([FromServices] IEmployeeService employeeService) =>
            {
                return Results.Ok(await employeeService.ListManagersAsync());
            })
            .WithName("GetAllManagers")
            .WithSummary("Retrieves all employees with the position of 'Manager'")
            .WithDescription("""
                Retrieves all employees who are designated as Managers.

                ### Responses
                - **200 OK**: Returns a list of employees with the position of 'Manager'.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<IEnumerable<EmployeeDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/employees/{employeeId}/average-order-amount", async (int employeeId, [FromServices] IEmployeeService employeeService, [FromServices] IOrderService orderService) =>
            {
                var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
                return Results.Ok(
                    new
                    {
                        employeeId,
                        averageOrderAmount = await orderService.CalculateAverageOrderAmountByEmployeeAsync(employeeId)
                    });
            })
            .WithName("GetAverageOrderAmountByEmployee")
            .WithSummary("Calculate average order amount for a specific employee")
            .WithDescription("""
                Calculates the average order amount handled by a specific employee.

                ### Path Parameters
                - **employeeId** (int, required): The ID of the employee.

                ### Responses
                - **200 OK**: Returns the average order amount for the specified employee.
                - **404 Not Found**: If the employee does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Employee")
            .Produces<object>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();
        }
    }
}