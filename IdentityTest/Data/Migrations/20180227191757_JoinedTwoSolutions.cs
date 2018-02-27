using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityTest.Data.Migrations
{
    public partial class JoinedTwoSolutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeelnemerSessies");

            migrationBuilder.DropTable(
                name: "Deelnemers");

            migrationBuilder.DropTable(
                name: "RegistratieVerzoeken");

            migrationBuilder.CreateTable(
                name: "ApplicationUserSessie",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    SessieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSessie", x => new { x.ApplicationUserId, x.SessieId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSessie_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSessie_Sessies_SessieId",
                        column: x => x.SessieId,
                        principalTable: "Sessies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSessie_SessieId",
                table: "ApplicationUserSessie",
                column: "SessieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserSessie");

            migrationBuilder.CreateTable(
                name: "RegistratieVerzoeken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: false),
                    Organisatie = table.Column<string>(nullable: true),
                    Telefoon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistratieVerzoeken", x => x.Id);
                    table.UniqueConstraint("AK_RegistratieVerzoeken_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Deelnemers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Naam = table.Column<string>(nullable: true),
                    Organisatie = table.Column<string>(nullable: true),
                    RegistratieVerzoekId = table.Column<int>(nullable: true),
                    Telefoon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deelnemers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deelnemers_RegistratieVerzoeken_RegistratieVerzoekId",
                        column: x => x.RegistratieVerzoekId,
                        principalTable: "RegistratieVerzoeken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeelnemerSessies",
                columns: table => new
                {
                    DeelnemerId = table.Column<int>(nullable: false),
                    SessieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeelnemerSessies", x => new { x.DeelnemerId, x.SessieId });
                    table.ForeignKey(
                        name: "FK_DeelnemerSessies_Deelnemers_DeelnemerId",
                        column: x => x.DeelnemerId,
                        principalTable: "Deelnemers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeelnemerSessies_Sessies_SessieId",
                        column: x => x.SessieId,
                        principalTable: "Sessies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deelnemers_RegistratieVerzoekId",
                table: "Deelnemers",
                column: "RegistratieVerzoekId",
                unique: true,
                filter: "[RegistratieVerzoekId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeelnemerSessies_SessieId",
                table: "DeelnemerSessies",
                column: "SessieId");
        }
    }
}
