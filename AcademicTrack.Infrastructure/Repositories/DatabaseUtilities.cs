using AcademicTrack.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;

namespace AcademicTrack.Infrastructure.Persistence.Repositories;

public class DatabaseUtilities : IDatabaseUtilities
{
    static DatabaseUtilities()
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    private readonly string _connectionString;

    public DatabaseUtilities(IConfiguration configuration)
    {
        _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? "Host=postgres-db;Port=5432;Database=academictrack;Username=academictrack;Password=academictrack_dev";
    }

    public async Task<List<T>> ExecuteQuery<T>(string query, DynamicParameters? parameters = null)
    {
        var connection = new NpgsqlConnection(_connectionString);
        try
        {
            var response = await connection.QueryAsync<T>(query, parameters);
            await connection.CloseAsync();
            return response.ToList();
        }
        catch (Exception)
        {
            await connection.CloseAsync();
            throw;
        }
    }

    public async Task<T?> ExecuteQuerySingle<T>(string query, DynamicParameters? parameters = null)
    {
        var connection = new NpgsqlConnection(_connectionString);
        try
        {
            return await connection.QuerySingleOrDefaultAsync<T>(query, parameters);
        }
        catch (Exception)
        {
            await connection.CloseAsync();
            throw;
        }
    }

    public async Task ExecuteCommandAsync(string command, object? parameters = null)
    {
        var connection = new NpgsqlConnection(_connectionString);
        try
        {
            await connection.ExecuteAsync(command, parameters);
            await connection.CloseAsync();
        }
        catch (Exception)
        {
            await connection.CloseAsync();
            throw;
        }
    }
}
