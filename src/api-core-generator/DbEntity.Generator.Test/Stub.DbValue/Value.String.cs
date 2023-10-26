using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateString(string value)
        =>
        InnerCreateString(value);

    internal static DbValue CreateNullableString(string? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateString(value);

    private static DbValue InnerCreateString(string value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetString() == value && db.Get() == (object)value));
}