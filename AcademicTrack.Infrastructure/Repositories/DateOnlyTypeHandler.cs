using System.Data;
using Dapper;

namespace AcademicTrack.Infrastructure.Persistence.Repositories;

// Dapper no soporta DateOnly de forma nativa (ni siquiera en esta versión); sin este handler
// falla con "The member X of type System.DateOnly cannot be used as a parameter value".
public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value) => value switch
    {
        DateOnly dateOnly => dateOnly,
        DateTime dateTime => DateOnly.FromDateTime(dateTime),
        _ => DateOnly.FromDateTime(Convert.ToDateTime(value))
    };
}
