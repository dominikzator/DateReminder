using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DateReminder.Migrations
{
    /// <inheritdoc />
    public partial class RemovedMessageNoteAddedTitleInReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageNote",
                table: "Reminders");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reminders");

            migrationBuilder.AddColumn<string>(
                name: "MessageNote",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
