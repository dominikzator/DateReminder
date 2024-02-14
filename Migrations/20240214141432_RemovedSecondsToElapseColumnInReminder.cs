using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSecondsToElapseColumnInReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondsToElapse",
                table: "Reminders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SecondsToElapse",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 1,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 2,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 3,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 4,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 5,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 6,
                column: "SecondsToElapse",
                value: 172800);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 7,
                column: "SecondsToElapse",
                value: 172800);
        }
    }
}
