using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityTest.Data.Migrations
{
    public partial class joinedtwoprojects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Maxima",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaxNonRDW = table.Column<int>(nullable: false),
                    MaxRDW = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maxima", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistratieVerzoeken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Digest = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: false),
                    Organisatie = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Telefoon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistratieVerzoeken", x => x.Id);
                    table.UniqueConstraint("AK_RegistratieVerzoeken_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Ruimtes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Capacity = table.Column<int>(nullable: false),
                    Naam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruimtes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tijdvakken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Einde = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Start = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tijdvakken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naam = table.Column<string>(nullable: true),
                    Rgb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
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
                name: "Sessies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naam = table.Column<string>(nullable: true),
                    RuimteId = table.Column<int>(nullable: false),
                    TijdvakId = table.Column<int>(nullable: false),
                    TrackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessies_Ruimtes_RuimteId",
                        column: x => x.RuimteId,
                        principalTable: "Ruimtes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessies_Tijdvakken_TijdvakId",
                        column: x => x.TijdvakId,
                        principalTable: "Tijdvakken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessies_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackTijdvakken",
                columns: table => new
                {
                    TrackID = table.Column<int>(nullable: false),
                    TijdvakID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackTijdvakken", x => new { x.TrackID, x.TijdvakID });
                    table.ForeignKey(
                        name: "FK_TrackTijdvakken_Tijdvakken_TijdvakID",
                        column: x => x.TijdvakID,
                        principalTable: "Tijdvakken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackTijdvakken_Tracks_TrackID",
                        column: x => x.TrackID,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sessies_RuimteId",
                table: "Sessies",
                column: "RuimteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessies_TijdvakId",
                table: "Sessies",
                column: "TijdvakId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessies_TrackId",
                table: "Sessies",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackTijdvakken_TijdvakID",
                table: "TrackTijdvakken",
                column: "TijdvakID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeelnemerSessies");

            migrationBuilder.DropTable(
                name: "Maxima");

            migrationBuilder.DropTable(
                name: "TrackTijdvakken");

            migrationBuilder.DropTable(
                name: "Deelnemers");

            migrationBuilder.DropTable(
                name: "Sessies");

            migrationBuilder.DropTable(
                name: "RegistratieVerzoeken");

            migrationBuilder.DropTable(
                name: "Ruimtes");

            migrationBuilder.DropTable(
                name: "Tijdvakken");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
