using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changed_whatsapp_account_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsAppInformationId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WhatsAppInformationId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);
        }
    }
}
