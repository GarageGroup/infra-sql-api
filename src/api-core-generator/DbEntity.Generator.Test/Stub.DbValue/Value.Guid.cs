using System;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateGuid(Guid value)
        =>
        InnerCreateGuid(value);

    internal static DbValue CreateNullableGuid(Guid? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateGuid(value.Value);

    private static DbValue InnerCreateGuid(Guid value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetGuid() == value && db.Get() == (object)value));
}