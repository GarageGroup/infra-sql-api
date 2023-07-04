using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubDbParameterCollection : DbParameterCollection
{
    private readonly List<object> parameters = new();

    public override int Count
        =>
        parameters.Count;

    public override object SyncRoot
        =>
        throw new NotImplementedException();

    public FlatArray<object> ToFlatArray()
        =>
        parameters.ToFlatArray();

    public override int Add(object value)
    {
        parameters.Add(value);
        return parameters.Count - 1;
    }

    public override void AddRange(Array values)
        =>
        throw new NotImplementedException();

    public override void Clear()
        =>
        parameters.Clear();

    public override bool Contains(object value)
        =>
        parameters.Contains(value);

    public override bool Contains(string value)
        =>
        throw new NotImplementedException();

    public override void CopyTo(Array array, int index)
        =>
        throw new NotImplementedException();

    public override IEnumerator GetEnumerator()
        =>
        parameters.GetEnumerator();

    public override int IndexOf(object value)
        =>
        parameters.IndexOf(value);

    public override int IndexOf(string parameterName)
        =>
        throw new NotImplementedException();

    public override void Insert(int index, object value)
        =>
        parameters.Insert(index, value);

    public override void Remove(object value)
        =>
        parameters.Remove(value);

    public override void RemoveAt(int index)
        =>
        parameters.RemoveAt(index);

    public override void RemoveAt(string parameterName)
        =>
        throw new NotImplementedException();

    protected override System.Data.Common.DbParameter GetParameter(int index)
        =>
        throw new NotImplementedException();

    protected override System.Data.Common.DbParameter GetParameter(string parameterName)
        =>
        throw new NotImplementedException();

    protected override void SetParameter(int index, System.Data.Common.DbParameter value)
        =>
        throw new NotImplementedException();

    protected override void SetParameter(string parameterName, System.Data.Common.DbParameter value)
        =>
        throw new NotImplementedException();
}