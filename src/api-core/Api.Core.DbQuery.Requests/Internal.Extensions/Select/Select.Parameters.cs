using System;
using System.Collections.Generic;
using System.Linq;

namespace GGroupp.Infra;

partial class DbQueryExtensions
{
    internal static FlatArray<DbParameter> BuildParameters(this DbSelectQuery query)
    {
        if (query.Filter is null && query.JoinedTables.IsEmpty)
        {
            return default;
        }

        var filterParameters = query.Filter?.GetFilterParameters().AsEnumerable() ?? Enumerable.Empty<DbParameter>();
        var joinFilters = query.JoinedTables.AsEnumerable().Select(GetFilter).SelectMany(GetParameters);

        return filterParameters.Concat(joinFilters).ToFlatArray();

        static IDbFilter GetFilter(DbJoinedTable dbJoinedTable)
            =>
            dbJoinedTable.Filter;

        static IEnumerable<DbParameter> GetParameters(IDbFilter dbFilter)
            =>
            dbFilter.GetFilterParameters().AsEnumerable();
    }
}