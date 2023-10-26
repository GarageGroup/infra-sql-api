using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

partial class StubDbValue
{
    internal static DbValue Create<T>(T value)
        where T : notnull
        =>
        InnerCreate(value);

    internal static DbValue CreateNullable<T>(T? value)
        where T : class
        =>
        value is null ? InnerCreateNull() : InnerCreate(value);

    internal static DbValue CreateNullableStruct<T>(T? value)
        where T : struct
        =>
        value is null ? InnerCreateNull() : InnerCreate(value.Value);

    private static DbValue InnerCreate<T>(T value)
        where T : notnull
    {
        var mock = new Mock<IDbValueProvider>();

        _ = mock.Setup(static db => db.IsNull()).Returns(false);
        _ = mock.Setup(static db => db.Get<T>()).Returns(value);
        _ = mock.Setup(static db => db.Get()).Returns(value);

        return new(mock.Object);
    }
}