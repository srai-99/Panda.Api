using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Panda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Appointments",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
