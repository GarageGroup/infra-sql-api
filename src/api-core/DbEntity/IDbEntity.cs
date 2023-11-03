﻿namespace GarageGroup.Infra;

public interface IDbEntity<TEntity>
{
    static abstract TEntity ReadEntity(IDbItem dbItem);
}