using Moq;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

internal static partial class StubDbValue
{
    private static DbValue InnerCreateNull()
        =>
        new(
            Mock.Of<IDbValueProvider>(db => db.IsNull() == true));
}