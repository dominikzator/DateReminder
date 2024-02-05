using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class SeededDataSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_UserSettingsId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "CreatedDate", "SecondsToElapse", "SecondsToNotify" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 172800, 864000 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Password", "UserName", "UserSettingsId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234", "heniu123", 1 });

            migrationBuilder.InsertData(
                table: "Reminders",
                columns: new[] { "Id", "CreatedDate", "Priority", "SecondsToElapse", "SecondsToNotify", "TargetDate", "Title", "UserId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 172800, 864000, new DateTime(2024, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Urodziny Sylwii", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_Id",
                table: "UserSettings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_Id",
                table: "Reminders",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSettings_UserSettingsId",
                table: "Users",
                column: "UserSettingsId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_UserSettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_Id",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_Id",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSettings_UserSettingsId",
                table: "Users",
                column: "UserSettingsId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
