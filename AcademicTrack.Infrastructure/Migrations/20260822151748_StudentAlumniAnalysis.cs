using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcademicTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentAlumniAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "perdida_asignatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    PeriodoId = table.Column<int>(type: "integer", nullable: false),
                    Asignatura = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Matriculados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Aprobados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Reprobados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PorcentajePerdida = table.Column<decimal>(type: "numeric(6,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perdida_asignatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perdida_asignatura_periodo_academico_PeriodoId",
                        column: x => x.PeriodoId,
                        principalTable: "periodo_academico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_perdida_asignatura_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seguimiento_cohorte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    PeriodoCohorteId = table.Column<int>(type: "integer", nullable: false),
                    SemestreSeguimiento = table.Column<int>(type: "integer", nullable: false),
                    Ingresaron = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Continuaron = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Cancelaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Repitentes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CambiosPrograma = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Desertores = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Graduados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguimiento_cohorte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguimiento_cohorte_periodo_academico_PeriodoCohorteId",
                        column: x => x.PeriodoCohorteId,
                        principalTable: "periodo_academico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_seguimiento_cohorte_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seguimiento_egresado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    AnioGraduacion = table.Column<short>(type: "smallint", nullable: false),
                    TotalEgresados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Empleados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EmpleadosRelacionadosCarrera = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EmpleadosNoRelacionadosCarrera = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TiempoPromedioConseguirEmpleoMeses = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    ContratoIndefinido = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ContratoTerminoFijo = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ContratoPrestacionServicios = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ContratoOtro = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ContinuanEstudios = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguimiento_egresado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguimiento_egresado_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "distribucion_egresado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeguimientoEgresadoId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distribucion_egresado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_distribucion_egresado_seguimiento_egresado_SeguimientoEgres~",
                        column: x => x.SeguimientoEgresadoId,
                        principalTable: "seguimiento_egresado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "uq_distribucion_egresado",
                table: "distribucion_egresado",
                columns: new[] { "SeguimientoEgresadoId", "Tipo", "Categoria" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_perdida_asignatura_PeriodoId",
                table: "perdida_asignatura",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "uq_perdida_asignatura",
                table: "perdida_asignatura",
                columns: new[] { "ProgramaId", "PeriodoId", "Asignatura" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguimiento_cohorte_PeriodoCohorteId",
                table: "seguimiento_cohorte",
                column: "PeriodoCohorteId");

            migrationBuilder.CreateIndex(
                name: "uq_seguimiento_cohorte",
                table: "seguimiento_cohorte",
                columns: new[] { "ProgramaId", "PeriodoCohorteId", "SemestreSeguimiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_seguimiento_egresado",
                table: "seguimiento_egresado",
                columns: new[] { "ProgramaId", "AnioGraduacion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "distribucion_egresado");

            migrationBuilder.DropTable(
                name: "perdida_asignatura");

            migrationBuilder.DropTable(
                name: "seguimiento_cohorte");

            migrationBuilder.DropTable(
                name: "seguimiento_egresado");
        }
    }
}
