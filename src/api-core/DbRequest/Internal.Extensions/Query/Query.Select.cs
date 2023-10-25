using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbSelectQuery query)
    {
        var queryBuilder = new StringBuilder("SELECT ");

        if (query.Top is not null && query.Offset is null)
        {
            queryBuilder = queryBuilder.Append("TOP ").Append(query.Top.Value).Append(' ');
        }

        if (query.SelectedFields.IsEmpty)
        {
            queryBuilder = queryBuilder.Append('*');
        }
        else
        {
            queryBuilder = queryBuilder.AppendJoined(query.SelectedFields.AsEnumerable());
        }

        queryBuilder = queryBuilder.Append(" FROM ").Append(query.TableName);
        if (string.IsNullOrWhiteSpace(query.TableAlias) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(query.TableAlias);
        }

        var joinQuery = query.JoinedTables.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(joinQuery) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(joinQuery);
        }

        var applyQuery = query.AppliedTables.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(applyQuery) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(applyQuery);
        }

        var filterQuery = query.Filter?.GetFilterSqlQuery();
        if (string.IsNullOrWhiteSpace(filterQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" WHERE ").Append(filterQuery);
        }

        if (query.GroupByFields.IsNotEmpty)
        {
            queryBuilder = queryBuilder.Append(" GROUP BY ").AppendJoined(query.GroupByFields.AsEnumerable());
        }

        var orderByQuery = query.Orders.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(orderByQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" ORDER BY ").Append(orderByQuery);
        }

        if (query.Offset is null)
        {
            return queryBuilder.ToString();
        }

        queryBuilder = queryBuilder.Append(" OFFSET ").Append(query.Offset.Value);
        if (query.Top is null)
        {
            return queryBuilder.ToString();
        }

        return queryBuilder.Append(" ROWS FETCH NEXT ").Append(query.Top.Value).Append(" ROWS ONLY").ToString();
    }

    internal static FlatArray<DbParameter> BuildParameters(this DbSelectQuery query)
    {
        if (query.Filter is null && query.JoinedTables.IsEmpty && query.AppliedTables.IsEmpty)
        {
            return default;
        }

        var filterParameters = query.Filter?.GetFilterParameters().AsEnumerable() ?? Enumerable.Empty<DbParameter>();
        var joinFilters = query.JoinedTables.AsEnumerable().Select(GetFilter).SelectMany(GetParameters);
        var applyParameters = query.AppliedTables.AsEnumerable().SelectMany(GetAppliedParameters);

        return filterParameters.Concat(joinFilters).Concat(applyParameters).ToFlatArray();

        static IDbFilter GetFilter(DbJoinedTable dbJoinedTable)
            =>
            dbJoinedTable.Filter;

        static IEnumerable<DbParameter> GetParameters(IDbFilter dbFilter)
            =>
            dbFilter.GetFilterParameters().AsEnumerable();

        static IEnumerable<DbParameter> GetAppliedParameters(DbAppliedTable dbAppliedTable)
            =>
            dbAppliedTable.SelectQuery.BuildParameters().AsEnumerable();
    }

    private static string BuildSqlQuery(this FlatArray<DbJoinedTable> joinedTables)
    {
        if (joinedTables.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var joinedTable in joinedTables)
        {
            if (builder.Length > 0)
            {
                builder = builder.Append(' ');
            }

            builder = builder.AppendJoinedTable(joinedTable);
        }

        return builder.ToString();
    }

    private static string BuildSqlQuery(this FlatArray<DbAppliedTable> appliedTables)
    {
        if (appliedTables.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var appliedTable in appliedTables)
        {
            if (builder.Length > 0)
            {
                builder = builder.Append(' ');
            }

            builder = builder.AppendAppliedTable(appliedTable);
        }

        return builder.ToString();
    }

    private static StringBuilder AppendJoinedTable(this StringBuilder builder, DbJoinedTable joinedTable)
        =>
        builder.Append(
            joinedTable.Type.GetName())
        .Append(
            " JOIN ")
        .Append(
            joinedTable.TableName)
        .Append(
            ' ')
        .Append(
            joinedTable.TableAlias)
        .Append(
            " ON ")
        .Append(
            joinedTable.Filter.GetFilterSqlQuery());

    private static StringBuilder AppendAppliedTable(this StringBuilder builder, DbAppliedTable appliedTable)
        =>
        builder.Append(
            appliedTable.Type.GetName())
        .Append(
            " APPLY (")
        .Append(
            appliedTable.SelectQuery.GetSqlQuery())
        .Append(
            ") ")
        .Append(
            appliedTable.Alias);
}