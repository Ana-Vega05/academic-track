namespace AcademicTrack.Domain.Entities;

public class IndicadorMatricula
{
    public int Id { get; set; }
    public int PeriodoId { get; set; }
    public int ProgramaId { get; set; }

    public int? Inscritos { get; set; }
    public int? Admitidos { get; set; }
    public decimal? TasaSelectividad { get; set; }      // DECIMAL(5,4)
    public int MatriculadosTotal { get; set; }
    public int? MatriculadosPrimerCurso { get; set; }
    public short? TransferenciaExterna { get; set; }
    public short? TransferenciaInterna { get; set; }
    public short? ExcluidosBajoRendimiento { get; set; }
    public short? RetiradosCancelacion { get; set; }
    public short? ReintegroMatricula { get; set; }
    public decimal? TasaAbsorcion { get; set; }         // DECIMAL(5,4)
    public int? TotalGraduados { get; set; }
    public decimal? PctCulminanCarrera { get; set; }    // DECIMAL(6,4)
    public decimal? TasaDesercionSpadies { get; set; }  // DECIMAL(6,4)
    public decimal? PctTasaDesercion { get; set; }      // DECIMAL(6,4)
    public decimal? PromedioSaberPro { get; set; }      // DECIMAL(6,2)
    public decimal? GrupoReferenciaSaberPro { get; set; }
    public decimal? MediaNacionalSaberPro { get; set; }
    public short MovilidadSalienteNacional { get; set; } = 0;
    public short MovilidadSalienteInternacional { get; set; } = 0;
    public short MovilidadEntranteNacional { get; set; } = 0;
    public short MovilidadEntranteInternacional { get; set; } = 0;
}