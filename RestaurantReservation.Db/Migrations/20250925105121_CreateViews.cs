using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW view_ReservationDetails AS
                SELECT 
                    Rev.ReservationId, Rev.ReservationDate, Rev.PartySize,
                    Res.Name, Res.Address,
                    Cus.FirstName, Cus.LastName
                FROM Reservations Rev
                JOIN Restaurants Res ON Res.RestaurantId = Rev.RestaurantId
                JOIN Customers Cus ON Cus.CustomerId = Rev.CustomerId;
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW view_EmployeeDetails AS
                SELECT 
                    Emp.EmployeeId, Emp.FirstName, Emp.LastName, Emp.Position,
                    Res.Name, Res.Address
                FROM Employees Emp
                JOIN Restaurants Res ON Res.RestaurantId = Emp.RestaurantId;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS view_ReservationDetails");
            migrationBuilder.Sql("DROP VIEW IF EXISTS view_EmployeeDetails");
        }
    }
}
