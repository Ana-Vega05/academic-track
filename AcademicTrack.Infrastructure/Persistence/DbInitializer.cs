using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(AcademicTrackDbContext context)
    {
        try
        {
            context.Database.EnsureCreated();

            if (!context.Programas.Any())
            {
                var programas = new[]
                {
                    new Programa { Id = 1, Nombre = "Ingeniería de Sistemas", Activo = true },
                    new Programa { Id = 2, Nombre = "Ingeniería Industrial", Activo = true },
                    new Programa { Id = 3, Nombre = "Ingeniería Electrónica", Activo = true }
                };
                context.Programas.AddRange(programas);
                context.SaveChanges();
            }

            if (!context.PeriodoAcademicos.Any())
            {
                var periodos = new List<PeriodoAcademico>();
                int idCounter = 1;
                for (short anio = 2025; anio >= 2018; anio--)
                {
                    periodos.Add(new PeriodoAcademico { Id = idCounter++, Anio = anio, Semestre = Semestre.I });
                    if (anio > 2018 || anio == 2025)
                    {
                        periodos.Add(new PeriodoAcademico { Id = idCounter++, Anio = anio, Semestre = Semestre.II });
                    }
                }
                context.PeriodoAcademicos.AddRange(periodos);
                context.SaveChanges();
            }

            if (!context.SeguimientosCohorte.Any())
            {
                var cohortes = new List<SeguimientoCohorte>();
                int idCounter = 1;

                for (int programaId = 1; programaId <= 3; programaId++)
                {
                    for (int periodoId = 1; periodoId <= 5; periodoId++)
                    {
                        for (int sem = 1; sem <= 10; sem++)
                        {
                            int ingresaron = 60;
                            int desertores = (sem * 2) + 3;
                            int continuaron = Math.Max(0, ingresaron - desertores - (sem > 8 ? 20 : 0));
                            int graduados = sem >= 9 ? 20 : 0;

                            cohortes.Add(new SeguimientoCohorte
                            {
                                Id = idCounter++,
                                ProgramaId = programaId,
                                PeriodoCohorteId = periodoId,
                                SemestreSeguimiento = sem,
                                Ingresaron = ingresaron,
                                Continuaron = continuaron,
                                Cancelaciones = 2,
                                Repitentes = 3,
                                CambiosPrograma = 1,
                                Desertores = desertores,
                                Graduados = graduados
                            });
                        }
                    }
                }
                context.SeguimientosCohorte.AddRange(cohortes);
                context.SaveChanges();
            }

            if (!context.SeguimientosEgresado.Any())
            {
                var egresados = new List<SeguimientoEgresado>();
                int idCounter = 1;

                for (int programaId = 1; programaId <= 3; programaId++)
                {
                    for (short anio = 2020; anio <= 2024; anio++)
                    {
                        egresados.Add(new SeguimientoEgresado
                        {
                            Id = idCounter++,
                            ProgramaId = programaId,
                            AnioGraduacion = anio,
                            TotalEgresados = 45,
                            Empleados = 38,
                            EmpleadosRelacionadosCarrera = 32,
                            EmpleadosNoRelacionadosCarrera = 6,
                            TiempoPromedioConseguirEmpleoMeses = 3.5m,
                            ContratoIndefinido = 25,
                            ContratoTerminoFijo = 8,
                            ContratoPrestacionServicios = 5,
                            ContratoOtro = 0,
                            ContinuanEstudios = 7
                        });
                    }
                }
                context.SeguimientosEgresado.AddRange(egresados);
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            // Seed exception handler
            Console.WriteLine($"Error al sembrar datos: {ex.Message}");
        }
    }
}
