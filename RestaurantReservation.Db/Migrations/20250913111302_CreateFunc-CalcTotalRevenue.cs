using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateFuncCalcTotalRevenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                    CREATE FUNCTION CalcTotalRevenue(@RestaurantId INT)
                    RETURNS DECIMAL (10,2)
                    AS
                    BEGIN
                        DECLARE @TotalRevenue DECIMAL(18,2);
                    
                        SELECT @TotalRevenue = SUM(Ord.TotalAmount)
                        FROM Orders Ord
                        INNER JOIN Reservations Rev ON Rev.ReservationId = Ord.ReservationId
                        WHERE Rev.RestaurantId = @RestaurantId;

                        RETURN ISNULL(@TotalRevenue, 0);
                    END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalcTotalRevenue");
        }
    }
}
