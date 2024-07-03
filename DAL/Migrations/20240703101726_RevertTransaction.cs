using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RevertTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Transactions_TransactionID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TransactionID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions",
                column: "OrderID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "TransactionID",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TransactionID",
                table: "Orders",
                column: "TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Transactions_TransactionID",
                table: "Orders",
                column: "TransactionID",
                principalTable: "Transactions",
                principalColumn: "TransactionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
