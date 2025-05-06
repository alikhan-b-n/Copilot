using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Altegio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Dialogue",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Timezone",
                table: "CopilotChatBots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AltegioAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserToken = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltegioAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AltegioAccounts_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AltegioCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<int>(type: "integer", nullable: false),
                    Timezone = table.Column<int>(type: "integer", nullable: false),
                    AltegioAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltegioCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AltegioCompanies_AltegioAccounts_AltegioAccountId",
                        column: x => x.AltegioAccountId,
                        principalTable: "AltegioAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AltegioAccounts_UserId1",
                table: "AltegioAccounts",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AltegioCompanies_AltegioAccountId",
                table: "AltegioCompanies",
                column: "AltegioAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltegioCompanies");

            migrationBuilder.DropTable(
                name: "AltegioAccounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Dialogue");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "CopilotChatBots");
        }
    }
}
