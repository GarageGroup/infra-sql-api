using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateFloat(float value)
        =>
        InnerCreateFloat(value);

    internal static DbValue CreateNullableFloat(float? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateFloat(value.Value);

    private static DbValue InnerCreateFloat(float value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetFloat() == value && db.Get() == (object)value));
}