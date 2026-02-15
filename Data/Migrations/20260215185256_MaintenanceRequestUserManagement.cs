using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyHub.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceRequestUserManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "MaintenanceRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "MaintenanceRequests");
        }
    }
}
