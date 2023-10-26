using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateInt64(long value)
        =>
        InnerCreateInt64(value);

    internal static DbValue CreateNullableInt64(long? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateInt64(value.Value);

    private static DbValue InnerCreateInt64(long value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt64() == value && db.Get() == (object)value));
}