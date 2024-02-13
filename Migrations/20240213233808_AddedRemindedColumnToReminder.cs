using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class AddedRemindedColumnToReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reminded",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 2,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 3,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 4,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 5,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 6,
                column: "Reminded",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reminders",
                keyColumn: "Id",
                keyValue: 7,
                column: "Reminded",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminded",
                table: "Reminders");
        }
    }
}
