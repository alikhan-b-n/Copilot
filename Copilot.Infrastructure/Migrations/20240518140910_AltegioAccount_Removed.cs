using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AltegioAccount_Removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltegioCompanies_AltegioAccounts_AltegioAccountId",
                table: "AltegioCompanies");

            migrationBuilder.DropTable(
                name: "AltegioAccounts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AltegioCompanies");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "AltegioCompanies");

            migrationBuilder.RenameColumn(
                name: "AltegioAccountId",
                table: "AltegioCompanies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AltegioCompanies_AltegioAccountId",
                table: "AltegioCompanies",
                newName: "IX_AltegioCompanies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AltegioCompanies_AspNetUsers_UserId",
                table: "AltegioCompanies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltegioCompanies_AspNetUsers_UserId",
                table: "AltegioCompanies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AltegioCompanies",
                newName: "AltegioAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AltegioCompanies_UserId",
                table: "AltegioCompanies",
                newName: "IX_AltegioCompanies_AltegioAccountId");

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "AltegioCompanies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Timezone",
                table: "AltegioCompanies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AltegioAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserToken = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AltegioAccounts_UserId1",
                table: "AltegioAccounts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AltegioCompanies_AltegioAccounts_AltegioAccountId",
                table: "AltegioCompanies",
                column: "AltegioAccountId",
                principalTable: "AltegioAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
