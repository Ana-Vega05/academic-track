using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcademicTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCompleto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "periodo_academico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anio = table.Column<short>(type: "smallint", nullable: false),
                    Semestre = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_periodo_academico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "programa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CodigoSnies = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Facultad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "actividad_extension",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    Anio = table.Column<short>(type: "smallint", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Coordinador = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    LogroResultados = table.Column<string>(type: "text", nullable: true),
                    ComunidadBeneficiada = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividad_extension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_actividad_extension_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "convenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    CiudadOPais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Institucion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Objeto = table.Column<string>(type: "text", nullable: false),
                    LogroResultados = table.Column<string>(type: "text", nullable: true),
                    NumUsuarios = table.Column<int>(type: "int", nullable: true),
                    Vigencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_convenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_convenio_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grupo_investigacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Sigla = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ClasificacionMinciencias = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    NumInvestigadores = table.Column<short>(type: "smallint", nullable: true),
                    TotalProductos = table.Column<int>(type: "int", nullable: true),
                    ArticulosIndexados = table.Column<int>(type: "int", nullable: true),
                    ArticulosRii = table.Column<int>(type: "int", nullable: true),
                    ArticulosRini = table.Column<int>(type: "int", nullable: true),
                    ArticulosRni = table.Column<int>(type: "int", nullable: true),
                    ArticulosRnni = table.Column<int>(type: "int", nullable: true),
                    LibrosCompletos = table.Column<int>(type: "int", nullable: true),
                    LibrosCapitulos = table.Column<int>(type: "int", nullable: true),
                    TrabajosGradoPregrado = table.Column<int>(type: "int", nullable: true),
                    TrabajosGradoMaestria = table.Column<int>(type: "int", nullable: true),
                    TrabajosGradoDoctorado = table.Column<int>(type: "int", nullable: true),
                    NumPatentes = table.Column<int>(type: "int", nullable: true),
                    OtrosResultados = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo_investigacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grupo_investigacion_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "indicador_matricula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PeriodoId = table.Column<int>(type: "integer", nullable: false),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    Inscritos = table.Column<int>(type: "int", nullable: true),
                    Admitidos = table.Column<int>(type: "int", nullable: true),
                    TasaSelectividad = table.Column<decimal>(type: "numeric(5,4)", nullable: true),
                    MatriculadosTotal = table.Column<int>(type: "integer", nullable: false),
                    MatriculadosPrimerCurso = table.Column<int>(type: "int", nullable: true),
                    TransferenciaExterna = table.Column<short>(type: "smallint", nullable: true),
                    TransferenciaInterna = table.Column<short>(type: "smallint", nullable: true),
                    ExcluidosBajoRendimiento = table.Column<short>(type: "smallint", nullable: true),
                    RetiradosCancelacion = table.Column<short>(type: "smallint", nullable: true),
                    ReintegroMatricula = table.Column<short>(type: "smallint", nullable: true),
                    TasaAbsorcion = table.Column<decimal>(type: "numeric(5,4)", nullable: true),
                    TotalGraduados = table.Column<int>(type: "int", nullable: true),
                    PctCulminanCarrera = table.Column<decimal>(type: "numeric(6,4)", nullable: true),
                    TasaDesercionSpadies = table.Column<decimal>(type: "numeric(6,4)", nullable: true),
                    PctTasaDesercion = table.Column<decimal>(type: "numeric(6,4)", nullable: true),
                    PromedioSaberPro = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    GrupoReferenciaSaberPro = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    MediaNacionalSaberPro = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    MovilidadSalienteNacional = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    MovilidadSalienteInternacional = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    MovilidadEntranteNacional = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    MovilidadEntranteInternacional = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indicador_matricula", x => x.Id);
                    table.ForeignKey(
                        name: "FK_indicador_matricula_periodo_academico_PeriodoId",
                        column: x => x.PeriodoId,
                        principalTable: "periodo_academico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_indicador_matricula_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "innovacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramaId = table.Column<int>(type: "integer", nullable: false),
                    Profesor = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FechaEntrega = table.Column<DateOnly>(type: "date", nullable: false),
                    EntidadBeneficiada = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ComunidadBeneficiaria = table.Column<string>(type: "text", nullable: true),
                    Impacto = table.Column<string>(type: "text", nullable: true),
                    TieneSoporte = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AplicacionUso = table.Column<string>(type: "text", nullable: true),
                    Anio = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_innovacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_innovacion_programa_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "programa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "actividad_extension_vinculacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadId = table.Column<int>(type: "integer", nullable: false),
                    TipoActor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividad_extension_vinculacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_actividad_extension_vinculacion_actividad_extension_Activid~",
                        column: x => x.ActividadId,
                        principalTable: "actividad_extension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "convenio_vinculacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    TipoActor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_convenio_vinculacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_convenio_vinculacion_convenio_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "convenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "linea_investigacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_linea_investigacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_linea_investigacion_grupo_investigacion_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "grupo_investigacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupoId = table.Column<int>(type: "integer", nullable: false),
                    TipoPublicacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Anio = table.Column<short>(type: "smallint", nullable: false),
                    ReferenciaCompleta = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Indexada = table.Column<bool>(type: "boolean", nullable: true),
                    BaseIndexacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cuartil = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_publicacion_grupo_investigacion_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "grupo_investigacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "innovacion_vinculacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InnovacionId = table.Column<int>(type: "integer", nullable: false),
                    TipoActor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Condicion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "Interno"),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_innovacion_vinculacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_innovacion_vinculacion_innovacion_InnovacionId",
                        column: x => x.InnovacionId,
                        principalTable: "innovacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "proyecto_investigacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupoId = table.Column<int>(type: "integer", nullable: false),
                    LineaId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    InvestigadorPrincipal = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ProductosGenerados = table.Column<string>(type: "text", nullable: true),
                    UrlProducto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ComunidadBeneficiada = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    AnioInicio = table.Column<short>(type: "smallint", nullable: true),
                    Vigente = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proyecto_investigacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_proyecto_investigacion_grupo_investigacion_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "grupo_investigacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_proyecto_investigacion_linea_investigacion_LineaId",
                        column: x => x.LineaId,
                        principalTable: "linea_investigacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "publicacion_autor",
                columns: table => new
                {
                    PublicacionId = table.Column<int>(type: "integer", nullable: false),
                    AutorId = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicacion_autor", x => new { x.PublicacionId, x.AutorId });
                    table.ForeignKey(
                        name: "FK_publicacion_autor_autor_AutorId",
                        column: x => x.AutorId,
                        principalTable: "autor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicacion_autor_publicacion_PublicacionId",
                        column: x => x.PublicacionId,
                        principalTable: "publicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "proyecto_investigacion_vinculacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProyectoId = table.Column<int>(type: "integer", nullable: false),
                    TipoActor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proyecto_investigacion_vinculacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_proyecto_investigacion_vinculacion_proyecto_investigacion_P~",
                        column: x => x.ProyectoId,
                        principalTable: "proyecto_investigacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_actividad_anio",
                table: "actividad_extension",
                column: "Anio");

            migrationBuilder.CreateIndex(
                name: "idx_actividad_programa",
                table: "actividad_extension",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "idx_actividad_vinculacion_actividad",
                table: "actividad_extension_vinculacion",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "uq_actividad_vinculacion",
                table: "actividad_extension_vinculacion",
                columns: new[] { "ActividadId", "TipoActor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_autor_NombreCompleto",
                table: "autor",
                column: "NombreCompleto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_convenio_programa",
                table: "convenio",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "idx_convenio_tipo_estado",
                table: "convenio",
                columns: new[] { "Tipo", "Estado" });

            migrationBuilder.CreateIndex(
                name: "idx_convenio_vinculacion_convenio",
                table: "convenio_vinculacion",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "uq_convenio_vinculacion",
                table: "convenio_vinculacion",
                columns: new[] { "ConvenioId", "TipoActor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grupo_investigacion_Nombre",
                table: "grupo_investigacion",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_grupo_programa",
                table: "grupo_investigacion",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "IX_indicador_matricula_PeriodoId",
                table: "indicador_matricula",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "idx_indicador_programa_periodo",
                table: "indicador_matricula",
                columns: new[] { "ProgramaId", "PeriodoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_innovacion_anio",
                table: "innovacion",
                column: "Anio");

            migrationBuilder.CreateIndex(
                name: "idx_innovacion_programa",
                table: "innovacion",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "idx_innovacion_vinculacion_innovacion",
                table: "innovacion_vinculacion",
                column: "InnovacionId");

            migrationBuilder.CreateIndex(
                name: "uq_innovacion_vinculacion",
                table: "innovacion_vinculacion",
                columns: new[] { "InnovacionId", "TipoActor", "Condicion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_linea_grupo",
                table: "linea_investigacion",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_periodo_academico_Anio_Semestre",
                table: "periodo_academico",
                columns: new[] { "Anio", "Semestre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programa_Nombre",
                table: "programa",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_proyecto_investigacion_LineaId",
                table: "proyecto_investigacion",
                column: "LineaId");

            migrationBuilder.CreateIndex(
                name: "idx_proyecto_grupo",
                table: "proyecto_investigacion",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "idx_proyecto_vigente",
                table: "proyecto_investigacion",
                column: "Vigente",
                filter: "\"Vigente\" = true");

            migrationBuilder.CreateIndex(
                name: "idx_proyecto_vinculacion_proyecto",
                table: "proyecto_investigacion_vinculacion",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "uq_proyecto_vinculacion",
                table: "proyecto_investigacion_vinculacion",
                columns: new[] { "ProyectoId", "TipoActor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_publicacion_anio",
                table: "publicacion",
                column: "Anio");

            migrationBuilder.CreateIndex(
                name: "idx_publicacion_grupo",
                table: "publicacion",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "idx_publicacionautor_autor",
                table: "publicacion_autor",
                column: "AutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actividad_extension_vinculacion");

            migrationBuilder.DropTable(
                name: "convenio_vinculacion");

            migrationBuilder.DropTable(
                name: "indicador_matricula");

            migrationBuilder.DropTable(
                name: "innovacion_vinculacion");

            migrationBuilder.DropTable(
                name: "proyecto_investigacion_vinculacion");

            migrationBuilder.DropTable(
                name: "publicacion_autor");

            migrationBuilder.DropTable(
                name: "actividad_extension");

            migrationBuilder.DropTable(
                name: "convenio");

            migrationBuilder.DropTable(
                name: "periodo_academico");

            migrationBuilder.DropTable(
                name: "innovacion");

            migrationBuilder.DropTable(
                name: "proyecto_investigacion");

            migrationBuilder.DropTable(
                name: "autor");

            migrationBuilder.DropTable(
                name: "publicacion");

            migrationBuilder.DropTable(
                name: "linea_investigacion");

            migrationBuilder.DropTable(
                name: "grupo_investigacion");

            migrationBuilder.DropTable(
                name: "programa");
        }
    }
}
