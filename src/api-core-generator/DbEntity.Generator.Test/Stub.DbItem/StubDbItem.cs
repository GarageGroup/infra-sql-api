using System.Collections.Generic;
using System.Linq;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

internal static class StubDbItem
{
    internal static IDbItem Create(IReadOnlyDictionary<string, DbValue> orThrowValues, IReadOnlyDictionary<string, DbValue?> orDefaultValues)
    {
        var mock = new Mock<IDbItem>();

        _ = mock.Setup(static db => db.GetFieldValueOrThrow(It.IsAny<string>())).Returns(InnerOrThrow);
        _ = mock.Setup(static db => db.GetFieldValueOrDefault(It.IsAny<string>())).Returns(InnerOrDefault);
        _ = mock.SetupGet(static db => db.Fields).Returns(orThrowValues.Keys.Concat(orDefaultValues.Keys));

        return mock.Object;

        DbValue InnerOrThrow(string fieldName)
            =>
            orThrowValues[fieldName];

        DbValue? InnerOrDefault(string fieldName)
            =>
            orDefaultValues[fieldName];
    }
}