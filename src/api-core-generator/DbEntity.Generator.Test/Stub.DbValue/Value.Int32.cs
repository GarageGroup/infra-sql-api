using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateInt32(int value)
        =>
        InnerCreateInt32(value);

    internal static DbValue CreateNullableInt32(int? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateInt32(value.Value);

    private static DbValue InnerCreateInt32(int value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt32() == value && db.Get() == (object)value));
}