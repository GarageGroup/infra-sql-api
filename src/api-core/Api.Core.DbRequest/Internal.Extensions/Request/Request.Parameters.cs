using System;
using System.Collections.Generic;
using System.Linq;

namespace GGroupp.Infra;

partial class DbRequestExtensions
{
    internal static FlatArray<DbParameter> BuildParameters(this DbRequest request)
    {
        if (request.Filters.IsEmpty && request.JoinedTables.IsEmpty)
        {
            return default;
        }

        var joinFilters = request.JoinedTables.AsEnumerable().SelectMany(GetFilters);
        return request.Filters.AsEnumerable().Concat(joinFilters).SelectMany(GetFilterParameters).ToFlatArray();

        static IEnumerable<IDbFilter> GetFilters(DbJoinedTable dbJoinedTable)
            =>
            dbJoinedTable.Filters.AsEnumerable();

        static IEnumerable<DbParameter> GetFilterParameters(IDbFilter dbFilter)
            =>
            dbFilter.GetFilterParameters().AsEnumerable();
    }
}