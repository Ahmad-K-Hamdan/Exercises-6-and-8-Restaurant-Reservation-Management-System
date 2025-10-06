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
            .WithTags("Customer")
            .Produces<IEnumerable<CustomerDTO>>(200)
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
            .WithTags("Customer")
            .Produces<CustomerDTO>(200)
            .Produces(404)
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
            .WithTags("Customer")
            .Produces<CustomerDTO>(201)
            .Produces(400)
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
            .WithTags("Customer")
            .Produces<CustomerDTO>(200)
            .Produces(400)
            .Produces(404)
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
            .WithTags("Customer")
            .Produces(204)
            .Produces(404)
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
            .WithTags("Customer")
            .Produces<IEnumerable<CustomerDetailsDTO>>(200)
            .Produces(400)
            .Produces(404)
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