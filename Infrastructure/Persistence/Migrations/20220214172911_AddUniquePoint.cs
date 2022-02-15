using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddUniquePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Points_ListId",
                table: "Points");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ListId_X_Y",
                table: "Points",
                columns: new[] { "ListId", "X", "Y" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Points_ListId_X_Y",
                table: "Points");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ListId",
                table: "Points",
                column: "ListId");
        }
    }
}
