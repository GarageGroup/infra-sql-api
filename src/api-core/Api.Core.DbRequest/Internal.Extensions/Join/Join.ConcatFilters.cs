using System;

namespace GGroupp.Infra;

partial class DbRequestExtensions
{
    internal static FlatArray<IDbFilter> Concat(this IDbFilter filter, IDbFilter[]? otherFilters)
    {
        if (otherFilters?.Length is not > 0)
        {
            return new(filter);
        }

        var filters = new IDbFilter[otherFilters.Length + 1];
        filters[0] = filter;

        for (var i = 0; i < otherFilters.Length; i++)
        {
            filters[i + 1] = otherFilters[i];
        }

        return filters;
    }
}