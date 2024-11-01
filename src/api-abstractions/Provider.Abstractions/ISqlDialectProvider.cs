namespace GarageGroup.Infra;

public interface ISqlDialectProvider
{
    SqlDialect Dialect { get; }
}