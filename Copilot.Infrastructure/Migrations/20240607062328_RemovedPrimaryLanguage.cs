using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPrimaryLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryLanguage",
                table: "CopilotChatBots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryLanguage",
                table: "CopilotChatBots",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
