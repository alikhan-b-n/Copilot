﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Copilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_whatsapp_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WhatsAppInformationId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsAppInformationId",
                table: "AspNetUsers");
        }
    }
}
