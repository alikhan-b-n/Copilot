using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogue_CopilotChatBots_CopilotChatBotId",
                table: "Dialogue");

            migrationBuilder.DropColumn(
                name: "ChatBotId",
                table: "Dialogue");

            migrationBuilder.AlterColumn<Guid>(
                name: "CopilotChatBotId",
                table: "Dialogue",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogue_CopilotChatBots_CopilotChatBotId",
                table: "Dialogue",
                column: "CopilotChatBotId",
                principalTable: "CopilotChatBots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogue_CopilotChatBots_CopilotChatBotId",
                table: "Dialogue");

            migrationBuilder.AlterColumn<Guid>(
                name: "CopilotChatBotId",
                table: "Dialogue",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatBotId",
                table: "Dialogue",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogue_CopilotChatBots_CopilotChatBotId",
                table: "Dialogue",
                column: "CopilotChatBotId",
                principalTable: "CopilotChatBots",
                principalColumn: "Id");
        }
    }
}
