using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateInt16(short value)
        =>
        InnerCreateInt16(value);

    internal static DbValue CreateNullableInt16(short? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateInt16(value.Value);

    private static DbValue InnerCreateInt16(short value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt16() == value && db.Get() == (object)value));
}