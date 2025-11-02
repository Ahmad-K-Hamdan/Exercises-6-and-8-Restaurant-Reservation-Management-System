using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateProcListAllCusWithMinPartySize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE ListAllCusWithMinPartySize
                    @MinPartySize INT
                AS
                BEGIN
                    SELECT 
                        Cus.CustomerId, Cus.FirstName, Cus.LastName,
                        Res.ReservationDate, Res.PartySize
                    FROM Customers Cus
                    JOIN Reservations Res ON Res.CustomerId = Cus.CustomerId
                    WHERE Res.PartySize > @MinPartySize;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE ListAllCusWithMinPartySize");
        }
    }
}
