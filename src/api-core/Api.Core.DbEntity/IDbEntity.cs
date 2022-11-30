namespace GGroupp.Infra;

#if NET7_0_OR_GREATER

public interface IDbEntity<TEntity>
{
    static abstract TEntity ReadEntity(IDbItem dbItem);
}

#endif