using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateDecimal(decimal value)
        =>
        InnerCreateDecimal(value);

    internal static DbValue CreateNullableDecimal(decimal? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateDecimal(value.Value);

    private static DbValue InnerCreateDecimal(decimal value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDecimal() == value && db.Get() == (object)value));
}