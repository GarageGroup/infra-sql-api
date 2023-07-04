using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubDbCommand : DbCommand
{
    private readonly IStubDbCommand innerDbCommand;

    internal StubDbCommand(IStubDbCommand innerDbCommand)
        =>
        this.innerDbCommand = innerDbCommand;

    [AllowNull]
    public override string CommandText { get; set; }

    public override int CommandTimeout { get; set; }

    public override CommandType CommandType { get; set; }

    public override bool DesignTimeVisible { get; set; }

    public override UpdateRowSource UpdatedRowSource { get; set; }

    protected override DbConnection? DbConnection { get; set; }

    protected override DbParameterCollection DbParameterCollection { get; }
        =
        new StubDbParameterCollection();

    protected override DbTransaction? DbTransaction { get; set; }

    public override void Cancel()
        =>
        throw new NotImplementedException();

    public override int ExecuteNonQuery()
        =>
        innerDbCommand.ExecuteNonQuery();

    public override object? ExecuteScalar()
        =>
        throw new NotImplementedException();

    public override void Prepare()
        =>
        throw new NotImplementedException();

    protected override System.Data.Common.DbParameter CreateDbParameter()
        =>
        throw new NotImplementedException();

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        =>
        innerDbCommand.ExecuteDbDataReader(behavior);
}