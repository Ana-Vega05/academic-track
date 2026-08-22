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
    public DbSet<ActividadExtension> ActividadesExtension => Set<ActividadExtension>();
    public DbSet<ActividadExtensionVinculacion> ActividadExtensionVinculaciones => Set<ActividadExtensionVinculacion>();
    public DbSet<Publicacion> Publicaciones => Set<Publicacion>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<PublicacionAutor> PublicacionAutores => Set<PublicacionAutor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}