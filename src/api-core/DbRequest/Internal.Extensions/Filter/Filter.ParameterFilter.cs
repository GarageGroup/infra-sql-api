using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildFilterSqlQuery(this DbParameterFilter filter)
        =>
        new StringBuilder()
        .Append(filter.FieldName)
        .Append(' ')
        .Append(filter.Operator.GetSign())
        .Append(' ')
        .Append('@')
        .Append(filter.ParameterName)
        .ToString();

    internal static FlatArray<DbParameter> BuildFilterParameters(this DbParameterFilter filter)
        =>
        new(
            new DbParameter(filter.ParameterName, filter.FieldValue));
}