using System;
using System.Reflection;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal static class TestHelper
{
    internal static T? GetInnerFieldValue<T>(this object source, string fieldName)
        =>
        (T?)source.GetType().GetInnerFieldInfoOrThrow(fieldName).GetValue(source);

    private static FieldInfo GetInnerFieldInfoOrThrow(this Type type, string fieldName)
        =>
        type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"An inner field '{fieldName}' of the type '{type.Name}' was not found");
}