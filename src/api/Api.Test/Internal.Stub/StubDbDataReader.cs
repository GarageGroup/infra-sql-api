using System;
using System.Collections;
using System.Data.Common;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubDbDataReader : DbDataReader
{
    private readonly IStubDbDataReader innerDbDataReader;

    internal StubDbDataReader(IStubDbDataReader innerDbDataReader)
        =>
        this.innerDbDataReader = innerDbDataReader;

    public override object this[int ordinal]
        =>
        innerDbDataReader.GetValue(ordinal);

    public override object this[string name]
        =>
        throw new NotImplementedException();

    public override int Depth
        =>
        throw new NotImplementedException();

    public override int FieldCount
        =>
        innerDbDataReader.FieldCount;

    public override bool HasRows
        =>
        throw new NotImplementedException();

    public override bool IsClosed
        =>
        throw new NotImplementedException();

    public override int RecordsAffected
        =>
        throw new NotImplementedException();

    public override bool GetBoolean(int ordinal)
        =>
        innerDbDataReader.GetBoolean(ordinal);

    public override byte GetByte(int ordinal)
        =>
        innerDbDataReader.GetByte(ordinal);

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        =>
        throw new NotImplementedException();

    public override char GetChar(int ordinal)
        =>
        throw new NotImplementedException();

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        =>
        throw new NotImplementedException();

    public override string GetDataTypeName(int ordinal)
        =>
        throw new NotImplementedException();

    public override DateTime GetDateTime(int ordinal)
        =>
        innerDbDataReader.GetDateTime(ordinal);

    public override decimal GetDecimal(int ordinal)
        =>
        innerDbDataReader.GetDecimal(ordinal);

    public override double GetDouble(int ordinal)
        =>
        innerDbDataReader.GetDouble(ordinal);

    public override IEnumerator GetEnumerator()
        =>
        throw new NotImplementedException();

    public override Type GetFieldType(int ordinal)
        =>
        throw new NotImplementedException();

    public override float GetFloat(int ordinal)
        =>
        innerDbDataReader.GetFloat(ordinal);

    public override Guid GetGuid(int ordinal)
        =>
        innerDbDataReader.GetGuid(ordinal);

    public override short GetInt16(int ordinal)
        =>
        innerDbDataReader.GetInt16(ordinal);

    public override int GetInt32(int ordinal)
        =>
        innerDbDataReader.GetInt32(ordinal);

    public override long GetInt64(int ordinal)
        =>
        innerDbDataReader.GetInt64(ordinal);

    public override string GetName(int ordinal)
        =>
        innerDbDataReader.GetName(ordinal);

    public override int GetOrdinal(string name)
        =>
        throw new NotImplementedException();

    public override string GetString(int ordinal)
        =>
        innerDbDataReader.GetString(ordinal);

    public override object GetValue(int ordinal)
        =>
        innerDbDataReader.GetValue(ordinal);

    public override int GetValues(object[] values)
        =>
        throw new NotImplementedException();

    public override bool IsDBNull(int ordinal)
        =>
        innerDbDataReader.IsDBNull(ordinal);

    public override bool NextResult()
        =>
        throw new NotImplementedException();

    public override bool Read()
        =>
        innerDbDataReader.Read();
}