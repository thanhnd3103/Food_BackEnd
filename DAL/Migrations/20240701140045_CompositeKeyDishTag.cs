using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompositeKeyDishTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishTags_DishID",
                table: "DishTags");

            migrationBuilder.CreateIndex(
                name: "IX_DishTags_DishID_TagID",
                table: "DishTags",
                columns: new[] { "DishID", "TagID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishTags_DishID_TagID",
                table: "DishTags");

            migrationBuilder.CreateIndex(
                name: "IX_DishTags_DishID",
                table: "DishTags",
                column: "DishID");
        }
    }
}
