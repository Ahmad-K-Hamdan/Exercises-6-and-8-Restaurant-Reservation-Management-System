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
                    return Results.NotFound();
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
                    return Results.BadRequest(new { error = ex.Message });
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
                var existing = await customerService.GetCustomerByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                var customer = await customerService.UpdateAsync(id, dto);
                return Results.Ok(ToDTO(customer));
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
                var existing = await customerService.GetCustomerByIdAsync(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }
                await customerService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteCustomer")
            .WithSummary("Deletes a customer by its ID")
            .WithTags("Customer")
            .Produces(204)
            .Produces(404)
            .RequireAuthorization();

            app.MapGet("/api/customers/party-size/{minPartySize:int}", async ([AsParameters] PartySizeDTO dto, [FromServices] ICustomerService customerService) =>
            {
                var customers = await customerService.FindCustomersByPartySizeAsync(dto.PartySize);
                if (customers == null || !customers.Any())
                {
                    return Results.NotFound();
                }
                return Results.Ok(customers);
            })
            .WithName("FindCustomersByPartySize")
            .WithSummary("Finds customers who have made reservations with a party size greater than or equal to the specified minimum")
            .WithTags("Customer")
            .Produces<IEnumerable<CustomerDetailsDTO>>(200)
            .Produces(400)
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