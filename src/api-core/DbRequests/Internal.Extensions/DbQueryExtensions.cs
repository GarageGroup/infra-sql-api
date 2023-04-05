using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGroupp.Infra;

internal static partial class DbQueryExtensions
{
    private static FlatArray<TResult> Map<TSource, TResult>(this FlatArray<TSource> source, Func<TSource, TResult> map)
    {
        var builder = FlatArray<TResult>.Builder.OfLength(source.Length);

        for (var i = 0; i < source.Length; i++)
        {
            builder[i] = map.Invoke(source[i]);
        }

        return builder.MoveToFlatArray();
    }

    private static FlatArray<T> Concat<T>(this FlatArray<T> source, FlatArray<T> other)
    {
        if (other.IsEmpty)
        {
            return source;
        }

        if (source.IsEmpty)
        {
            return other;
        }

        var builder = FlatArray<T>.Builder.OfLength(source.Length + other.Length);

        for (var i = 0; i < source.Length; i++)
        {
            builder[i] = source[i];
        }

        for (var i = 0; i < other.Length; i++)
        {
            builder[source.Length + i] = other[i];
        }

        return builder.MoveToFlatArray();
    }

    private static string BuildSqlQuery(this FlatArray<IDbFilter> filters)
    {
        if (filters.IsEmpty)
        {
            return string.Empty;
        }

        if (filters.Length is 1)
        {
            return filters[0].GetFilterSqlQuery().Trim();
        }

        var builder = new StringBuilder();

        foreach (var filter in filters)
        {
            var filterSqlQuery = filter.GetFilterSqlQuery();
            if (string.IsNullOrWhiteSpace(filterSqlQuery))
            {
                continue;
            }

            if (builder.Length > 0)
            {
                builder = builder.Append(" AND ");
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

    private static string BuildSqlQuery(this FlatArray<DbOrder> dbOrderSet)
    {
        if (dbOrderSet.IsEmpty)
        {
            return string.Empty;
        }

        return new StringBuilder().AppendJoined(dbOrderSet.AsEnumerable().Select(InnerBuildSqlQuery)).ToString();

        static string InnerBuildSqlQuery(DbOrder dbOrder)
        {
            var name = dbOrder.OrderType.GetName();
            return string.IsNullOrEmpty(name) ? dbOrder.FieldName : $"{dbOrder.FieldName} {name}";
        }
    }

    private static StringBuilder AppendJoined(this StringBuilder builder, IEnumerable<string> collection)
    {
        var values = string.Join(", ", collection.Where(IsNotWhiteSpace));
        return builder.Append(values);

        static bool IsNotWhiteSpace(string source)
            =>
            string.IsNullOrWhiteSpace(source) is false;
    }

    private static StringBuilder AppendSeparator(this StringBuilder builder, string separator = ", ")
        =>
        builder.Length > 0 ? builder.Append(separator) : builder;

    private static DbParameter MapFieldValue(DbFieldValue fieldValue)
        =>
        new(fieldValue.ParameterName, fieldValue.FieldValue);

    private static string GetSign(this DbArrayFilterOperator @operator)
        =>
        @operator switch
        {
            DbArrayFilterOperator.In => "IN",
            DbArrayFilterOperator.NotIn => "NOT IN",
            _ => throw OutOfRangeException(@operator)
        };

    private static string GetSign(this DbFilterOperator @operator)
        =>
        @operator switch
        {
            DbFilterOperator.Equal => "=",
            DbFilterOperator.Greater => ">",
            DbFilterOperator.GreaterOrEqual => ">=",
            DbFilterOperator.Less => "<",
            DbFilterOperator.LessOrEqual => "<=",
            DbFilterOperator.Inequal => "<>",
            _ => throw OutOfRangeException(@operator)
        };

    private static string GetName(this DbLogicalOperator @operator)
        =>
        @operator switch
        {
            DbLogicalOperator.And => "AND",
            DbLogicalOperator.Or => "OR",
            _ => throw OutOfRangeException(@operator),
        };

    private static string GetName(this DbJoinType joinType)
        =>
        joinType switch
        {
            DbJoinType.Inner => "INNER",
            DbJoinType.Left => "LEFT",
            DbJoinType.Right => "RIGHT",
            _ => throw OutOfRangeException(joinType),
        };

    private static string GetName(this DbOrderType orderType)
        =>
        orderType switch
        {
            DbOrderType.Default => string.Empty,
            DbOrderType.Ascending => "ASC",
            DbOrderType.Descending => "DESC",
            _ => throw OutOfRangeException(orderType),
        };

    private static string GetName(this DbApplyType applyType)
        =>
        applyType switch
        {
            DbApplyType.Outer => "OUTER",
            DbApplyType.Cross => "CROSS",
            _ => throw OutOfRangeException(applyType),
        };

    private static ArgumentOutOfRangeException OutOfRangeException<T>(T orderType)
        =>
        new($"An unexpected {typeof(T).FullName} value: {orderType}");
}