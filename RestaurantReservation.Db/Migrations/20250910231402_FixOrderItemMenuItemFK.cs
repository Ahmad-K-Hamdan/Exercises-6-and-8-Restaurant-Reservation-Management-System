using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItemMenuItemFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_MenuItemItemId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "MenuItemItemId",
                table: "OrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemId",
                table: "OrderItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_ItemId",
                table: "OrderItems",
                column: "ItemId",
                principalTable: "MenuItems",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_ItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemItemId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemItemId",
                table: "OrderItems",
                column: "MenuItemItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemId",
                table: "OrderItems",
                column: "MenuItemItemId",
                principalTable: "MenuItems",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
