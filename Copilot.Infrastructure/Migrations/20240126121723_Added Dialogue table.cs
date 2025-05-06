using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDialoguetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssistantId",
                table: "CopilotChatBots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Dialogue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ThreadId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatBotId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CopilotChatBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dialogue_CopilotChatBots_CopilotChatBotId",
                        column: x => x.CopilotChatBotId,
                        principalTable: "CopilotChatBots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_CopilotChatBotId",
                table: "Dialogue",
                column: "CopilotChatBotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dialogue");

            migrationBuilder.DropColumn(
                name: "AssistantId",
                table: "CopilotChatBots");
        }
    }
}
