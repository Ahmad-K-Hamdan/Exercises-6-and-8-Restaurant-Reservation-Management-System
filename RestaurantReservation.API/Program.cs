using RestaurantReservation.API.Endpoints;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Interfaces;
using RestaurantReservation.Services;
using RestaurantReservation.Services.Interfaces;

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

            // Add API Explorer services required for Swagger to discover endpoints
            builder.Services.AddEndpointsApiExplorer();

            // Add Swagger generator to create OpenAPI documentation
            builder.Services.AddSwaggerGen();

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

            // Map endpoints
            app.MapTableEndpoints();

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