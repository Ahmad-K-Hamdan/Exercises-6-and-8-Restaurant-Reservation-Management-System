using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.Customer;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Services.Interfaces;

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
                var customer = await customerService.AddAsync(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);
                return Results.Created($"/api/customers/{customer.CustomerId}", ToDTO(customer));
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
                var customer = await customerService.UpdateAsync(id, dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);
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