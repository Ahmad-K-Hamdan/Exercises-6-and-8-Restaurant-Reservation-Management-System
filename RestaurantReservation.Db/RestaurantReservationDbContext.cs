using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Shared.DTOs.Employee;
using RestaurantReservation.Shared.DTOs.Reservation;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        public DbSet<ReservationDetailsDTO> ReservationDetailsView { get; set; }
        public DbSet<EmployeeDetailsDTO> EmployeeDetailsView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = RestaurantReservationCore"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // The primary key in MenuItem model does not follow EF rules
            // so it has to be defined manually
            modelBuilder.Entity<MenuItem>().HasKey(menuItem => menuItem.ItemId);
            // Follow: change the FK in OrderItems to be the correct one
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Orders deleted when Reservation is deleted
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Reservation)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.ReservationId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // OrderItems deleted when Order is deleted
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // No action for deletion: restaurant -> reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Restaurant)
                .WithMany(rt => rt.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure reservation details View
            modelBuilder.Entity<ReservationDetailsDTO>(entity =>
            {
                entity.ToView("view_ReservationDetails");
                entity.HasNoKey();
            });

            // Configure employees details View
            modelBuilder.Entity<EmployeeDetailsDTO>(entity =>
            {
                entity.ToView("view_EmployeeDetails");
                entity.HasNoKey();
            });

            // Configure the CalcTotalRevenue function 
            modelBuilder.HasDbFunction(() => CalcTotalRevenue(default))
                        .HasName("CalcTotalRevenue")
                        .HasSchema("dbo");
        }

        public decimal CalcTotalRevenue(int restaurantId)
        {
            throw new NotSupportedException("This method can only be used using a LINQ query.");
        }
    }
}
