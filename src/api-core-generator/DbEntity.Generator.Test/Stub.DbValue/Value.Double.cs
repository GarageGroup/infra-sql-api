using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateDouble(double value)
        =>
        InnerCreateDouble(value);

    internal static DbValue CreateNullableDouble(double? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateDouble(value.Value);

    private static DbValue InnerCreateDouble(double value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDouble() == value && db.Get() == (object)value));
}