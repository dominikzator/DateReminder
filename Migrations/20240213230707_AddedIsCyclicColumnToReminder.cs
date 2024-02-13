using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsCyclicColumnToReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCyclic",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsCyclic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsCyclic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsCyclic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsCyclic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsCyclic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsCyclic",
                value: true);

            migrationBuilder.InsertData(
                table: "Reminders",
                columns: new[] { "Id", "CreatedDate", "IsCyclic", "Priority", "SecondsToElapse", "SecondsToNotify", "TargetDate", "Title", "UserId" },
                values: new object[] { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4, 172800, 864000, new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fryzjer", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "IsCyclic",
                table: "Reminders");
        }
    }
}
