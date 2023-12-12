namespace GarageGroup.Infra;

public interface ISqlApi : ISqlExecuteNonQuerySupplier, ISqlQueryEntitySupplier, ISqlQueryEntitySetSupplier, IPingSupplier
{
}