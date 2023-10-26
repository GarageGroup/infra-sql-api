using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateBoolean(bool value)
        =>
        InnerCreateBoolean(value);

    internal static DbValue CreateNullableBoolean(bool? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateBoolean(value.Value);

    private static DbValue InnerCreateBoolean(bool value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetBoolean() == value && db.Get() == (object)value));
}