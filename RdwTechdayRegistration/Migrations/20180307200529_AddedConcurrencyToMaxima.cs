using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RdwTechdayRegistration.Migrations
{
    public partial class AddedConcurrencyToMaxima : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForceChangeCount",
                table: "Maxima",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Maxima",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForceChangeCount",
                table: "Maxima");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Maxima");
        }
    }
}
