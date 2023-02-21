namespace GGroupp.Infra;

public interface ISqlApi : ISqlExecuteNonQuerySupplier, ISqlQueryEntitySupplier, ISqlQueryEntitySetSupplier
{
}