using System;
using System.Collections.Generic;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildFilterSqlQuery(this DbCombinedFilter filter)
    {
        if (filter.Filters.IsEmpty)
        {
            return string.Empty;
        }

        if (filter.Filters.Length is 1)
        {
            return filter.Filters[0].GetFilterSqlQuery().Trim();
        }

        var builder = new StringBuilder();
        var operatorName = filter.Operator.GetName();

        foreach (var innerFilter in filter.Filters)
        {
            var filterSqlQuery = innerFilter.GetFilterSqlQuery();
            if (string.IsNullOrWhiteSpace(filterSqlQuery))
            {
                continue;
            }

            if (builder.Length > 0)
            {
                builder = builder.Append(' ').Append(operatorName).Append(' ');
            }
            else
            {
                builder = builder.Append('(');
            }

            builder.Append(filterSqlQuery);
        }

        if (builder.Length > 0)
        {
            builder = builder.Append(')');
        }

        return builder.ToString();
    }

    internal static FlatArray<DbParameter> BuildFilterParameters(this DbCombinedFilter filter)
    {
        if (filter.Filters.IsEmpty)
        {
            return default;
        }

        var list = new List<DbParameter>();

        foreach (var innerFilter in filter.Filters)
        {
            var filterParameters = innerFilter.GetFilterParameters();
            if (filterParameters.IsEmpty)
            {
                continue;
            }

            list.AddRange(filterParameters.AsEnumerable());
        }

        return list;
    }
}