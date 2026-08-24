using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcademicTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "indicador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Unidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indicador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "meta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    IndicadorId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Responsable = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Periodicidad = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaLimite = table.Column<DateOnly>(type: "date", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ValorEsperado = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    AvanceActual = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false, defaultValue: "NoIniciada")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meta_indicador_IndicadorId",
                        column: x => x.IndicadorId,
                        principalTable: "indicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_meta_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "meta_evidencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MetaId = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCarga = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meta_evidencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meta_evidencia_meta_MetaId",
                        column: x => x.MetaId,
                        principalTable: "meta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_indicador_Nombre",
                table: "indicador",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meta_IndicadorId",
                table: "meta",
                column: "IndicadorId");

            migrationBuilder.CreateIndex(
                name: "idx_meta_estado",
                table: "meta",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "idx_meta_fecha_limite",
                table: "meta",
                column: "FechaLimite");

            migrationBuilder.CreateIndex(
                name: "idx_meta_programa",
                table: "meta",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "idx_meta_evidencia_meta",
                table: "meta_evidencia",
                column: "MetaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "meta_evidencia");

            migrationBuilder.DropTable(
                name: "meta");

            migrationBuilder.DropTable(
                name: "indicador");
        }
    }
}
