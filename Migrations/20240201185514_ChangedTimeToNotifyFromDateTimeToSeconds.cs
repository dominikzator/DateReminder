using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTimeToNotifyFromDateTimeToSeconds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToElapse",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TimeToNotify",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TimeToElapse",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "TimeToNotify",
                table: "Reminders");

            migrationBuilder.AddColumn<int>(
                name: "SecondsToElapse",
                table: "UserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondsToNotify",
                table: "UserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondsToElapse",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondsToNotify",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondsToElapse",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SecondsToNotify",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SecondsToElapse",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "SecondsToNotify",
                table: "Reminders");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToElapse",
                table: "UserSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToNotify",
                table: "UserSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToElapse",
                table: "Reminders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToNotify",
                table: "Reminders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
