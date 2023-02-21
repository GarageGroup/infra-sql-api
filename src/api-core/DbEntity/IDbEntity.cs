namespace GGroupp.Infra;

public interface IDbEntity<TEntity>
{
#if NET7_0_OR_GREATER
    static abstract TEntity ReadEntity(IDbItem dbItem);
#endif
}