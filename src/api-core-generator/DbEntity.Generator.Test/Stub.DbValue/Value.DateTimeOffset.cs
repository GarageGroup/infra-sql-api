using System;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateDateTimeOffset(DateTimeOffset value)
        =>
        InnerCreateDateTimeOffset(value);

    internal static DbValue CreateNullableDateTimeOffset(DateTimeOffset? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateDateTimeOffset(value.Value);

    private static DbValue InnerCreateDateTimeOffset(DateTimeOffset value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTimeOffset() == value && db.Get() == (object)value));
}