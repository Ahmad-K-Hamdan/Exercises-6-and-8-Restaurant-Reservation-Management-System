using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantReservation.API.Auth;
using RestaurantReservation.API.Endpoints;
using RestaurantReservation.Core.Services;
using RestaurantReservation.Core.Services.Interfaces;
using RestaurantReservation.Core.Validators.CustomerValidators;
using RestaurantReservation.Core.Validators.EmployeeValidators;
using RestaurantReservation.Core.Validators.MenuItemValidators;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Shared.DTOs.Customer;
using RestaurantReservation.Shared.DTOs.Employee;

namespace RestaurantReservation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a web application builder
            var builder = WebApplication.CreateBuilder(args);

            // Register all services with the builder
            ConfigureServices(builder.Services);

            // Register JWT Token Generator
            builder.Services.AddSingleton<JwtTokenGenerator>();

            // Add Authentication & Authorization
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });
            builder.Services.AddAuthorization();

            // Add API Explorer services required for Swagger to discover endpoints
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Restaurant Reservation API",
                    Version = "v1",
                    Description = "API for managing restaurant reservations with JWT authentication"
                });

                // JWT Authentication in Swagger
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Build the web application
            var app = builder.Build();

            // Ensure database is created and connection is established
            if (!InitializeDatabase(app.Services))
            {
                return;
            }

            // Configure the HTTP request pipeline and enable swagger in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirect HTTP requests to HTTPS for security
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPost("/api/auth/token", (TokenRequest request, JwtTokenGenerator jwtGen) =>
            {
                var token = jwtGen.GenerateToken(request.Username, request.Email);
                return Results.Ok(new TokenResponse(token, DateTime.UtcNow.AddHours(1)));
            })
            .WithName("GenerateToken")
            .WithSummary("Generate JWT token for authentication")
            .WithTags("Authentication")
            .AllowAnonymous();

            // Map endpoints
            app.MapTableEndpoints();
            app.MapEmployeeEndpoints();
            app.MapRestaurantEndpoints();
            app.MapCustomerEndpoints();
            app.MapMenuItemEndpoints();
            app.MapOrderEndpoints();
            app.MapOrderItemEndpoints();
            app.MapReservationEndpoints();

            // Start the web app
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<RestaurantReservationDbContext>();

            // Register Repository Interfaces with their Implementations
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<ITableRepository, TableRepository>();

            // Register Service Interfaces with their Implementations
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<ITableService, TableService>();

            // Register Validators
            services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCustomerValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateEmployeeValidator>();
            services.AddValidatorsFromAssemblyContaining<PartySizeValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateMenuItemValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateMenuItemValidator>();
        }

        private static bool InitializeDatabase(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<RestaurantReservationDbContext>();
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while connecting to the database: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}