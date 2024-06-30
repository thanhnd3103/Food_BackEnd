using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeStatusAndAddField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Txn_ref",
                table: "Transactions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Txn_ref",
                table: "Transactions");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
