using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InstructionFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstructionFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CopilotChatBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructionFiles_CopilotChatBots_CopilotChatBotId",
                        column: x => x.CopilotChatBotId,
                        principalTable: "CopilotChatBots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructionFiles_CopilotChatBotId",
                table: "InstructionFiles",
                column: "CopilotChatBotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructionFiles");
        }
    }
}
