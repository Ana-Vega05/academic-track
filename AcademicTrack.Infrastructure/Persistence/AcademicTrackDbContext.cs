using System.Text.Json;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Persistence;

public class AcademicTrackDbContext : DbContext
{
    public AcademicTrackDbContext(DbContextOptions<AcademicTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Programa> Programas => Set<Programa>();
    public DbSet<PeriodoAcademico> PeriodoAcademicos => Set<PeriodoAcademico>();
    public DbSet<Convenio> Convenios => Set<Convenio>();
    public DbSet<ConvenioVinculacion> ConvenioVinculaciones => Set<ConvenioVinculacion>();
    public DbSet<GrupoInvestigacion> GruposInvestigacion => Set<GrupoInvestigacion>();
    public DbSet<LineaInvestigacion> LineasInvestigacion => Set<LineaInvestigacion>();
    public DbSet<ProyectoInvestigacion> ProyectosInvestigacion => Set<ProyectoInvestigacion>();
    public DbSet<ProyectoInvestigacionVinculacion> ProyectoInvestigacionVinculaciones => Set<ProyectoInvestigacionVinculacion>();
    public DbSet<Innovacion> Innovaciones => Set<Innovacion>();
    public DbSet<InnovacionVinculacion> InnovacionVinculaciones => Set<InnovacionVinculacion>();
    public DbSet<IndicadorMatricula> IndicadoresMatricula => Set<IndicadorMatricula>();
    public DbSet<SeguimientoCohorte> SeguimientosCohorte => Set<SeguimientoCohorte>();
    public DbSet<PerdidaAsignatura> PerdidasAsignatura => Set<PerdidaAsignatura>();
    public DbSet<SeguimientoEgresado> SeguimientosEgresado => Set<SeguimientoEgresado>();
    public DbSet<DistribucionEgresado> DistribucionesEgresado => Set<DistribucionEgresado>();
    public DbSet<ActividadExtension> ActividadesExtension => Set<ActividadExtension>();
    public DbSet<ActividadExtensionVinculacion> ActividadExtensionVinculaciones => Set<ActividadExtensionVinculacion>();
    public DbSet<Publicacion> Publicaciones => Set<Publicacion>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<PublicacionAutor> PublicacionAutores => Set<PublicacionAutor>();
    public DbSet<Indicador> Indicadores => Set<Indicador>();
    public DbSet<Meta> Metas => Set<Meta>();
    public DbSet<MetaEvidencia> MetaEvidencias => Set<MetaEvidencia>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityEvidence> ActivityEvidences => Set<ActivityEvidence>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


modelBuilder.Entity<SeguimientoCohorte>(entity =>
{
    entity.ToTable("seguimiento_cohorte");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.SemestreSeguimiento)
        .IsRequired();

    entity.Property(e => e.Ingresaron)
        .HasDefaultValue(0);

    entity.Property(e => e.Continuaron)
        .HasDefaultValue(0);

    entity.Property(e => e.Cancelaciones)
        .HasDefaultValue(0);

    entity.Property(e => e.Repitentes)
        .HasDefaultValue(0);

    entity.Property(e => e.CambiosPrograma)
        .HasDefaultValue(0);

    entity.Property(e => e.Desertores)
        .HasDefaultValue(0);

    entity.Property(e => e.Graduados)
        .HasDefaultValue(0);

    entity.HasOne<Programa>()
        .WithMany()
        .HasForeignKey(e => e.ProgramaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne<PeriodoAcademico>()
        .WithMany()
        .HasForeignKey(e => e.PeriodoCohorteId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasIndex(
        e => new
        {
            e.ProgramaId,
            e.PeriodoCohorteId,
            e.SemestreSeguimiento
        },
        "uq_seguimiento_cohorte"
    ).IsUnique();
});

modelBuilder.Entity<PerdidaAsignatura>(entity =>
{
    entity.ToTable("perdida_asignatura");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.Asignatura)
        .IsRequired()
        .HasMaxLength(200);

    entity.Property(e => e.Matriculados)
        .HasDefaultValue(0);

    entity.Property(e => e.Aprobados)
        .HasDefaultValue(0);

    entity.Property(e => e.Reprobados)
        .HasDefaultValue(0);

    entity.Property(e => e.PorcentajePerdida)
        .HasColumnType("decimal(6,4)");

    entity.HasOne<Programa>()
        .WithMany()
        .HasForeignKey(e => e.ProgramaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne<PeriodoAcademico>()
        .WithMany()
        .HasForeignKey(e => e.PeriodoId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasIndex(
        e => new
        {
            e.ProgramaId,
            e.PeriodoId,
            e.Asignatura
        },
        "uq_perdida_asignatura"
    ).IsUnique();
});

modelBuilder.Entity<SeguimientoEgresado>(entity =>
{
    entity.ToTable("seguimiento_egresado");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.AnioGraduacion)
        .HasColumnType("smallint");

    entity.Property(e => e.TotalEgresados)
        .HasDefaultValue(0);

    entity.Property(e => e.Empleados)
        .HasDefaultValue(0);

    entity.Property(e => e.EmpleadosRelacionadosCarrera)
        .HasDefaultValue(0);

    entity.Property(e => e.EmpleadosNoRelacionadosCarrera)
        .HasDefaultValue(0);

    entity.Property(e => e.TiempoPromedioConseguirEmpleoMeses)
        .HasColumnType("decimal(6,2)");

    entity.Property(e => e.ContratoIndefinido)
        .HasDefaultValue(0);

    entity.Property(e => e.ContratoTerminoFijo)
        .HasDefaultValue(0);

    entity.Property(e => e.ContratoPrestacionServicios)
        .HasDefaultValue(0);

    entity.Property(e => e.ContratoOtro)
        .HasDefaultValue(0);

    entity.Property(e => e.ContinuanEstudios)
        .HasDefaultValue(0);

    entity.HasOne<Programa>()
        .WithMany()
        .HasForeignKey(e => e.ProgramaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasIndex(
        e => new
        {
            e.ProgramaId,
            e.AnioGraduacion
        },
        "uq_seguimiento_egresado"
    ).IsUnique();
});

modelBuilder.Entity<DistribucionEgresado>(entity =>
{
    entity.ToTable("distribucion_egresado");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.Tipo)
        .IsRequired()
        .HasMaxLength(30);

    entity.Property(e => e.Categoria)
        .IsRequired()
        .HasMaxLength(200);

    entity.Property(e => e.Cantidad)
        .HasDefaultValue(0);

    entity.HasOne<SeguimientoEgresado>()
        .WithMany()
        .HasForeignKey(e => e.SeguimientoEgresadoId)
        .OnDelete(DeleteBehavior.Cascade);

    entity.HasIndex(
        e => new
        {
            e.SeguimientoEgresadoId,
            e.Tipo,
            e.Categoria
        },
        "uq_distribucion_egresado"
    ).IsUnique();
});


        modelBuilder.Entity<Programa>(entity =>
        {
            entity.ToTable("programa");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.Property(e => e.CodigoSnies).HasMaxLength(20);
            entity.Property(e => e.Facultad).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<PeriodoAcademico>(entity =>
        {
            entity.ToTable("periodo_academico");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Anio).IsRequired();
            entity.Property(e => e.Semestre)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(2);
            entity.HasIndex(e => new { e.Anio, e.Semestre }).IsUnique();
        });

        modelBuilder.Entity<Convenio>(entity =>
        {
            entity.ToTable("convenio");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CiudadOPais).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Institucion).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Objeto).IsRequired().HasColumnType("text");
            entity.Property(e => e.LogroResultados).HasColumnType("text");
            entity.Property(e => e.NumUsuarios).HasColumnType("int");
            entity.Property(e => e.Vigencia).HasMaxLength(100);
            entity.Property(e => e.Tipo).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Estado).IsRequired().HasConversion<string>().HasMaxLength(20);

            entity.HasOne<Programa>()
                  .WithMany()
                  .HasForeignKey(e => e.ProgramaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProgramaId, "idx_convenio_programa");
            entity.HasIndex(e => new { e.Tipo, e.Estado }, "idx_convenio_tipo_estado");
        });

        modelBuilder.Entity<ConvenioVinculacion>(entity =>
        {
            entity.ToTable("convenio_vinculacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoActor).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Cantidad).HasDefaultValue((short)0);

            entity.HasOne<Convenio>()
                  .WithMany()
                  .HasForeignKey(e => e.ConvenioId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ConvenioId, "idx_convenio_vinculacion_convenio");
            entity.HasIndex(e => new { e.ConvenioId, e.TipoActor }, "uq_convenio_vinculacion").IsUnique();
        });

        modelBuilder.Entity<GrupoInvestigacion>(entity =>
        {
            entity.ToTable("grupo_investigacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Sigla).HasMaxLength(20);
            entity.Property(e => e.ClasificacionMinciencias).HasConversion<string>().HasMaxLength(2);
            entity.Property(e => e.NumInvestigadores).HasColumnType("smallint");
            entity.Property(e => e.TotalProductos).HasColumnType("int");
            entity.Property(e => e.ArticulosIndexados).HasColumnType("int");
            entity.Property(e => e.ArticulosRii).HasColumnType("int");
            entity.Property(e => e.ArticulosRini).HasColumnType("int");
            entity.Property(e => e.ArticulosRni).HasColumnType("int");
            entity.Property(e => e.ArticulosRnni).HasColumnType("int");
            entity.Property(e => e.LibrosCompletos).HasColumnType("int");
            entity.Property(e => e.LibrosCapitulos).HasColumnType("int");
            entity.Property(e => e.TrabajosGradoPregrado).HasColumnType("int");
            entity.Property(e => e.TrabajosGradoMaestria).HasColumnType("int");
            entity.Property(e => e.TrabajosGradoDoctorado).HasColumnType("int");
            entity.Property(e => e.NumPatentes).HasColumnType("int");
            entity.Property(e => e.OtrosResultados).HasColumnType("int");

            entity.HasOne<Programa>()
                  .WithMany()
                  .HasForeignKey(e => e.ProgramaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.HasIndex(e => e.ProgramaId, "idx_grupo_programa");
        });

        modelBuilder.Entity<LineaInvestigacion>(entity =>
        {
            entity.ToTable("linea_investigacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);

            entity.HasOne<GrupoInvestigacion>()
                  .WithMany()
                  .HasForeignKey(e => e.GrupoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.GrupoId, "idx_linea_grupo");
        });

        modelBuilder.Entity<ProyectoInvestigacion>(entity =>
        {
            entity.ToTable("proyecto_investigacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(300);
            entity.Property(e => e.InvestigadorPrincipal).IsRequired().HasMaxLength(150);
            entity.Property(e => e.ProductosGenerados).HasColumnType("text");
            entity.Property(e => e.UrlProducto).HasMaxLength(500);
            entity.Property(e => e.ComunidadBeneficiada).HasMaxLength(300);
            entity.Property(e => e.AnioInicio).HasColumnType("smallint");
            entity.Property(e => e.Vigente).HasDefaultValue(true);

            entity.HasOne<GrupoInvestigacion>()
                  .WithMany()
                  .HasForeignKey(e => e.GrupoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<LineaInvestigacion>()
                  .WithMany()
                  .HasForeignKey(e => e.LineaId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.GrupoId, "idx_proyecto_grupo");
            entity.HasIndex(e => e.Vigente, "idx_proyecto_vigente")
                  .HasFilter("\"Vigente\" = true");
        });

        modelBuilder.Entity<ProyectoInvestigacionVinculacion>(entity =>
        {
            entity.ToTable("proyecto_investigacion_vinculacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoActor).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Cantidad).HasDefaultValue((short)0);

            entity.HasOne<ProyectoInvestigacion>()
                  .WithMany()
                  .HasForeignKey(e => e.ProyectoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProyectoId, "idx_proyecto_vinculacion_proyecto");
            entity.HasIndex(e => new { e.ProyectoId, e.TipoActor }, "uq_proyecto_vinculacion").IsUnique();
        });

        modelBuilder.Entity<Innovacion>(entity =>
        {
            entity.ToTable("innovacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Profesor).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(300);
            entity.Property(e => e.FechaEntrega).HasColumnType("date");
            entity.Property(e => e.EntidadBeneficiada).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ComunidadBeneficiaria).HasColumnType("text");
            entity.Property(e => e.Impacto).HasColumnType("text");
            entity.Property(e => e.TieneSoporte).HasDefaultValue(false);
            entity.Property(e => e.AplicacionUso).HasColumnType("text");
            entity.Property(e => e.Anio).HasColumnType("smallint");

            entity.HasOne<Programa>()
                  .WithMany()
                  .HasForeignKey(e => e.ProgramaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProgramaId, "idx_innovacion_programa");
            entity.HasIndex(e => e.Anio, "idx_innovacion_anio");
            // Nota: el índice GIN con pg_trgm lo dejamos pendiente por simplicidad
        });

        modelBuilder.Entity<InnovacionVinculacion>(entity =>
        {
            entity.ToTable("innovacion_vinculacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoActor).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Condicion)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(10)
                  .HasDefaultValue(CondicionActor.Interno);
            entity.Property(e => e.Cantidad).HasDefaultValue((short)0);

            entity.HasOne<Innovacion>()
                  .WithMany()
                  .HasForeignKey(e => e.InnovacionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.InnovacionId, "idx_innovacion_vinculacion_innovacion");
            entity.HasIndex(e => new { e.InnovacionId, e.TipoActor, e.Condicion }, "uq_innovacion_vinculacion").IsUnique();
        });

        modelBuilder.Entity<IndicadorMatricula>(entity =>
        {
            entity.ToTable("indicador_matricula");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Inscritos).HasColumnType("int");
            entity.Property(e => e.Admitidos).HasColumnType("int");
            entity.Property(e => e.TasaSelectividad).HasColumnType("decimal(5,4)");
            entity.Property(e => e.MatriculadosTotal).IsRequired();
            entity.Property(e => e.MatriculadosPrimerCurso).HasColumnType("int");
            entity.Property(e => e.TransferenciaExterna).HasColumnType("smallint");
            entity.Property(e => e.TransferenciaInterna).HasColumnType("smallint");
            entity.Property(e => e.ExcluidosBajoRendimiento).HasColumnType("smallint");
            entity.Property(e => e.RetiradosCancelacion).HasColumnType("smallint");
            entity.Property(e => e.ReintegroMatricula).HasColumnType("smallint");
            entity.Property(e => e.TasaAbsorcion).HasColumnType("decimal(5,4)");
            entity.Property(e => e.TotalGraduados).HasColumnType("int");
            entity.Property(e => e.PctCulminanCarrera).HasColumnType("decimal(6,4)");
            entity.Property(e => e.TasaDesercionSpadies).HasColumnType("decimal(6,4)");
            entity.Property(e => e.PctTasaDesercion).HasColumnType("decimal(6,4)");
            entity.Property(e => e.PromedioSaberPro).HasColumnType("decimal(6,2)");
            entity.Property(e => e.GrupoReferenciaSaberPro).HasColumnType("decimal(6,2)");
            entity.Property(e => e.MediaNacionalSaberPro).HasColumnType("decimal(6,2)");
            entity.Property(e => e.MovilidadSalienteNacional).HasDefaultValue((short)0);
            entity.Property(e => e.MovilidadSalienteInternacional).HasDefaultValue((short)0);
            entity.Property(e => e.MovilidadEntranteNacional).HasDefaultValue((short)0);
            entity.Property(e => e.MovilidadEntranteInternacional).HasDefaultValue((short)0);

            entity.HasOne<Programa>()
                  .WithMany()
                  .HasForeignKey(e => e.ProgramaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<PeriodoAcademico>()
                  .WithMany()
                  .HasForeignKey(e => e.PeriodoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ProgramaId, e.PeriodoId }, "idx_indicador_programa_periodo").IsUnique();
        });

        modelBuilder.Entity<ActividadExtension>(entity =>
        {
            entity.ToTable("actividad_extension");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Anio).IsRequired();
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Coordinador).HasMaxLength(150);
            entity.Property(e => e.LogroResultados).HasColumnType("text");
            entity.Property(e => e.ComunidadBeneficiada).HasDefaultValue(0);

            entity.HasOne<Programa>()
                  .WithMany()
                  .HasForeignKey(e => e.ProgramaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProgramaId, "idx_actividad_programa");
            entity.HasIndex(e => e.Anio, "idx_actividad_anio");
        });

        modelBuilder.Entity<ActividadExtensionVinculacion>(entity =>
        {
            entity.ToTable("actividad_extension_vinculacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoActor).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Cantidad).HasDefaultValue((short)0);

            entity.HasOne<ActividadExtension>()
                  .WithMany()
                  .HasForeignKey(e => e.ActividadId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ActividadId, "idx_actividad_vinculacion_actividad");
            entity.HasIndex(e => new { e.ActividadId, e.TipoActor }, "uq_actividad_vinculacion").IsUnique();
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.ToTable("publicacion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoPublicacion).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Anio).IsRequired();
            entity.Property(e => e.ReferenciaCompleta).IsRequired().HasColumnType("text");
            entity.Property(e => e.Url).HasMaxLength(500);
            entity.Property(e => e.BaseIndexacion).HasMaxLength(100);
            entity.Property(e => e.Cuartil).HasMaxLength(2);

            entity.HasOne<GrupoInvestigacion>()
                  .WithMany()
                  .HasForeignKey(e => e.GrupoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.GrupoId, "idx_publicacion_grupo");
            entity.HasIndex(e => e.Anio, "idx_publicacion_anio");
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.ToTable("autor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.NombreCompleto).IsUnique();
        });

        modelBuilder.Entity<PublicacionAutor>(entity =>
        {
            entity.ToTable("publicacion_autor");
            entity.HasKey(e => new { e.PublicacionId, e.AutorId });

            entity.HasOne<Publicacion>()
                  .WithMany()
                  .HasForeignKey(e => e.PublicacionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Autor>()
                  .WithMany()
                  .HasForeignKey(e => e.AutorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.AutorId, "idx_publicacionautor_autor");
        });
        
        modelBuilder.Entity<Indicador>(entity =>
        {
            entity.ToTable("indicador");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Unidad).HasMaxLength(20);
            entity.Property(e => e.Direccion).IsRequired().HasConversion<string>().HasMaxLength(15);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<Meta>(entity =>
        {
            entity.ToTable("meta");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Responsable).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Periodicidad).IsRequired().HasConversion<string>().HasMaxLength(15);
            entity.Property(e => e.FechaInicio).IsRequired();
            entity.Property(e => e.FechaLimite).IsRequired();
            entity.Property(e => e.ValorInicial).HasColumnType("decimal(10,2)");
            entity.Property(e => e.ValorEsperado).HasColumnType("decimal(10,2)");
            entity.Property(e => e.AvanceActual).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Estado).IsRequired().HasConversion<string>().HasMaxLength(15)
                .HasDefaultValue(EstadoMeta.NoIniciada);

            entity.HasOne<Programa>().WithMany().HasForeignKey(e => e.ProgramaId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Indicador>().WithMany().HasForeignKey(e => e.IndicadorId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProgramaId, "idx_meta_programa");
            entity.HasIndex(e => e.Estado, "idx_meta_estado");
            entity.HasIndex(e => e.FechaLimite, "idx_meta_fecha_limite");
        });

        modelBuilder.Entity<MetaEvidencia>(entity =>
        {
            entity.ToTable("meta_evidencia");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Url).HasMaxLength(500);
            entity.Property(e => e.FechaCarga).IsRequired();

            entity.HasOne<Meta>().WithMany().HasForeignKey(e => e.MetaId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.MetaId, "idx_meta_evidencia_meta");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("activity");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Responsible).IsRequired().HasMaxLength(150);
            entity.Property(e => e.ParticipatingProfessors)
                .HasColumnType("text")
                .HasConversion(
                    v => v.Count == 0 ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => string.IsNullOrWhiteSpace(v) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
            entity.Property(e => e.ParticipatingStudents)
                .HasColumnType("text")
                .HasConversion(
                    v => v.Count == 0 ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => string.IsNullOrWhiteSpace(v) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());
            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne<Programa>().WithMany().HasForeignKey(e => e.ProgramId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProgramId, "idx_activity_program");
            entity.HasIndex(e => e.Type, "idx_activity_type");
            entity.HasIndex(e => e.Date, "idx_activity_date");
        });

        modelBuilder.Entity<ActivityEvidence>(entity =>
        {
            entity.ToTable("activity_evidence");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.Property(e => e.UploadDate).IsRequired();

            entity.HasOne<Activity>().WithMany().HasForeignKey(e => e.ActivityId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.ActivityId, "idx_activity_evidence_activity");
        });
    }
}