using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue CreateByte(byte value)
        =>
        InnerCreateByte(value);

    internal static DbValue CreateNullableByte(byte? value)
        =>
        value is null ? InnerCreateNull() : InnerCreateByte(value.Value);

    private static DbValue InnerCreateByte(byte value)
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetByte() == value && db.Get() == (object)value));
}