using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RdwTechdayRegistration.Migrations
{
    public partial class RemovedTrackTijdVak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackTijdvakken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_TrackTijdvakken_TijdvakID",
                table: "TrackTijdvakken",
                column: "TijdvakID");
        }
    }
}
