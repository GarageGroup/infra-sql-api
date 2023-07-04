using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubDbConnection : DbConnection
{
    private readonly IStubDbConnection innerDbConnection;

    internal StubDbConnection(IStubDbConnection innerDbConnection)
        =>
        this.innerDbConnection = innerDbConnection;

    [AllowNull]
    public override string ConnectionString { get; set; }

    public override string Database
        =>
        throw new NotImplementedException();

    public override string DataSource
        =>
        throw new NotImplementedException();

    public override string ServerVersion
        =>
        throw new NotImplementedException();

    public override ConnectionState State
        =>
        throw new NotImplementedException();

    public override void ChangeDatabase(string databaseName)
        =>
        throw new NotImplementedException();

    public override void Close()
        =>
        throw new NotImplementedException();

    public override void Open()
        =>
        innerDbConnection.Open();

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        =>
        throw new NotImplementedException();

    protected override DbCommand CreateDbCommand()
        =>
        innerDbConnection.CreateDbCommand();
}