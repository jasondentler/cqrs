using System;
using System.Collections.Generic;
using System.Data;

namespace Cqrs.EventStore.MsSql.FluentSqlClient
{
    public interface ITransactionSyntax
    {
        IExecutableSyntax Scalar<TReturn>(string query, Action<TReturn> action);
        IExecutableSyntax Scalar<TReturn>(string query, IDictionary<string, object> values, Action<TReturn> action);
        IExecutableSyntax Scalar<TReturn>(Func<IDbCommand> buildCommand, Action<TReturn> action);

        IExecutableSyntax Reader(string query, Action<IDataReader> forEachRow);
        IExecutableSyntax Reader(string query, IDictionary<string, object> values, Action<IDataReader> forEachRow);
        IExecutableSyntax Reader(Func<IDbCommand> buildCommand, Action<IDataReader> forEachRow);

        IExecutableSyntax NonQuery(string query, Action<int> rowsAffected);
        IExecutableSyntax NonQuery(string query, IDictionary<string, object> values, Action<int> rowsAffected);
        IExecutableSyntax NonQuery(Func<IDbCommand> buildCommand, Action<int> rowsAffected);
        IExecutableSyntax NonQuery(string query);
        IExecutableSyntax NonQuery(string query, IDictionary<string, object> values);
        IExecutableSyntax NonQuery(Func<IDbCommand> buildCommand);

    }
}
