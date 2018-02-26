using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityTest.Data.Migrations
{
    public partial class AddColsToApplicationuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Digest",
                table: "RegistratieVerzoeken");

            migrationBuilder.DropColumn(
                name: "State",
                table: "RegistratieVerzoeken");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organisation",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Organisation",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Digest",
                table: "RegistratieVerzoeken",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "RegistratieVerzoeken",
                nullable: false,
                defaultValue: 0);
        }
    }
}
