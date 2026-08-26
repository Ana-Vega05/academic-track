using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcademicTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActivitiesAndSeedIndicators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Responsible = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ParticipatingProfessors = table.Column<string>(type: "text", nullable: true),
                    ParticipatingStudents = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_programa_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_evidence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UploadDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_evidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_evidence_activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_activity_date",
                table: "activity",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "idx_activity_program",
                table: "activity",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "idx_activity_type",
                table: "activity",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "idx_activity_evidence_activity",
                table: "activity_evidence",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_evidence");

            migrationBuilder.DropTable(
                name: "activity");
        }
    }
}
