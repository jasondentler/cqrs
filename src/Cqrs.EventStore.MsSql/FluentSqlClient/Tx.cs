using System;
using Cqrs.EventStore.MsSql.FluentSqlClient.Impl;

namespace Cqrs.EventStore.MsSql.FluentSqlClient
{
    public static class Tx
    {

        public static ITransactionSyntax With(string connectionString)
        {
            return new MsSqlSyntax(connectionString);
        }

    }
}
