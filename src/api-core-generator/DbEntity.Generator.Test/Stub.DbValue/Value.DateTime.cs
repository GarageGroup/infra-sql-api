using System;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateDateTime(DateTime value)
        =>
        InnerCreateDateTime(value);

    internal static DbValue CreateNullableDateTime(DateTime? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateDateTime(value.Value);

    private static DbValue InnerCreateDateTime(DateTime value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTime() == value && db.Get() == (object)value));
}