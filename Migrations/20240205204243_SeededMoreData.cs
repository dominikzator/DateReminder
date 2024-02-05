using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class SeededMoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reminders",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Reminders",
                columns: new[] { "Id", "CreatedDate", "Priority", "SecondsToElapse", "SecondsToNotify", "TargetDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pierwszy Dzień Wiosny", 1 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wigilia", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Password", "UserName", "UserSettingsId" },
                values: new object[,]
                {
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234", "stasiu123", 1 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4321", "mieciu321", 1 }
                });

            migrationBuilder.InsertData(
                table: "Reminders",
                columns: new[] { "Id", "CreatedDate", "Priority", "SecondsToElapse", "SecondsToNotify", "TargetDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rocznica Odzyskania Niepodległości", 2 },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Święto Pracy", 2 },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Światowy Dzień Pizzy", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reminders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
