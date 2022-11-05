using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateTime GetDateTime()
        =>
        dbDataReader.GetDateTime(fieldIndex);
}