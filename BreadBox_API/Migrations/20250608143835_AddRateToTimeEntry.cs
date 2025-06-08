using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreadBox_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRateToTimeEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "TimeEntries",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "TimeEntries");
        }
    }
}
