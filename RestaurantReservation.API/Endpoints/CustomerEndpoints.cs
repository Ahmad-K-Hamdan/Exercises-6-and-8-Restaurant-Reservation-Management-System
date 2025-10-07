using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Customer;

namespace RestaurantReservation.API.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this WebApplication app)
        {
            app.MapGet("/api/customers", async ([FromServices] ICustomerService customerService) =>
            {
                var customers = await customerService.ViewAllAsync();
                var customerDTOs = customers.Select(ToDTO).ToList();
                return Results.Ok(customerDTOs);
            })
            .WithName("GetAllCustomers")
            .WithSummary("Retrieves all customers")
            .WithDescription("""
                Returns a list of all registered customers in the system.

                ### Responses
                - **200 OK**: Returns a list of customers.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces<IEnumerable<CustomerDTO>>(200)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/customers/{id:int}", async (int id, [FromServices] ICustomerService customerService) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return Results.NotFound(new { error = $"Customer with ID {id} not found." });
                }
                return Results.Ok(ToDTO(customer));
            })
            .WithName("GetCustomerById")
            .WithSummary("Retrieves a customer by its ID")
            .WithDescription("""
                Fetches a specific customer's details using their unique ID.

                ### Parameters
                - **id** (int): The customer's unique identifier.

                ### Responses
                - **200 OK**: Returns the customer details.
                - **404 Not Found**: If no customer exists with the given ID.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces<CustomerDTO>(200)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapPost("/api/customers", async ([FromBody] CreateCustomerDTO dto, [FromServices] ICustomerService customerService) =>
            {
                try
                {
                    var customer = await customerService.AddAsync(dto);
                    return Results.Created($"/api/customers/{customer.CustomerId}", ToDTO(customer));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
            })
            .WithName("AddCustomer")
            .WithSummary("Creates a new customer")
            .WithDescription("""
                Adds a new customer to the system after validating input data.

                ### Request Body
                - **FirstName** (string, required)
                - **LastName** (string, required)
                - **Email** (string, required)
                - **PhoneNumber** (string, optional)

                ### Responses
                - **201 Created**: Returns the newly created customer.
                - **400 Bad Request**: If validation fails.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces<CustomerDTO>(201)
            .Produces(400)
            .Produces(401)
            .RequireAuthorization();

            app.MapPut("/api/customers/{id:int}", async (int id, [FromBody] UpdateCustomerDTO dto, [FromServices] ICustomerService customerService) =>
            {
                try
                {
                    var customer = await customerService.UpdateAsync(id, dto);
                    return Results.Ok(ToDTO(customer));
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
            .WithName("UpdateCustomer")
            .WithSummary("Updates an existing customer")
            .WithDescription("""
                Updates an existing customer's profile using their ID.

                ### Parameters
                - **id** (int): The customer's unique identifier.

                ### Request Body
                - **FirstName** (string, required)
                - **LastName** (string, required)
                - **Email** (string, required)
                - **PhoneNumber** (string, optional)

                ### Responses
                - **200 OK**: Returns the updated customer details.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If the customer does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces<CustomerDTO>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapDelete("/api/customers/{id:int}", async (int id, [FromServices] ICustomerService customerService) =>
            {
                try
                {
                    await customerService.DeleteAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("DeleteCustomer")
            .WithSummary("Deletes a customer by its ID")
            .WithDescription("""
                Removes a customer from the system.

                ### Parameters
                - **id** (int): The customer's unique identifier.

                ### Responses
                - **204 No Content**: Customer successfully deleted.
                - **404 Not Found**: If the customer does not exist.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces(204)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();

            app.MapGet("/api/customers/party-size", async ([AsParameters] PartySizeDTO dto, [FromServices] ICustomerService customerService) =>
            {
                try
                {
                    var customers = await customerService.FindCustomersByPartySizeAsync(dto.PartySize);
                    if (customers == null || !customers.Any())
                    {
                        return Results.NotFound(new { error = $"No customers found with party size >= {dto.PartySize}." });
                    }
                    return Results.Ok(customers);
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
            })
            .WithName("FindCustomersByPartySize")
            .WithSummary("Finds customers who have made reservations with a party size greater than the specified minimum")
            .WithDescription("""
                Retrieves all customers who have made reservations for a party size greater than or equal to the specified value.

                ### Query Parameters
                - **PartySize** (int, required): The minimum number of people in a reservation.

                ### Responses
                - **200 OK**: Returns a list of matching customers.
                - **400 Bad Request**: If validation fails.
                - **404 Not Found**: If no matching customers are found.
                - **401 Unauthorized**: If the user is not authenticated.
                """)
            .WithTags("Customer")
            .Produces<IEnumerable<CustomerDetailsDTO>>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401)
            .RequireAuthorization();
        }

        private static CustomerDTO ToDTO(Customer customer)
        {
            return new CustomerDTO(
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber
            );
        }
    }
}