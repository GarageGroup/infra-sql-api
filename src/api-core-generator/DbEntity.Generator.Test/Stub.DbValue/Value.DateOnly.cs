using System;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateDateOnly(DateOnly value)
        =>
        InnerCreateDateOnly(value);

    internal static DbValue CreateNullableDateOnly(DateOnly? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateDateOnly(value.Value);

    private static DbValue InnerCreateDateOnly(DateOnly value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateOnly() == value && db.Get() == (object)value));
}