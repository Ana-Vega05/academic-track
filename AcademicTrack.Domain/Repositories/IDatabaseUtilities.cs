using Dapper;

namespace AcademicTrack.Domain.Repositories;

public interface IDatabaseUtilities
{
    Task<List<T>> ExecuteQuery<T>(string query, DynamicParameters? parameters = null);
    Task<T?> ExecuteQuerySingle<T>(string query, DynamicParameters? parameters = null);
    Task ExecuteCommandAsync(string query, object? parameters = null);
}