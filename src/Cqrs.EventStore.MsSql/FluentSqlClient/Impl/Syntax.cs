using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Cqrs.EventStore.MsSql.FluentSqlClient.Impl
{

    public abstract class Syntax : IExecutableSyntax
    {
        private readonly Func<IDbConnection> _getConnection;
        private readonly Func<IDbCommand> _getCommand;
        private readonly Action<IDbCommand, string, object> _addParameter;

        protected Syntax(Func<IDbConnection> getConnection, Func<IDbCommand> getCommand, Action<IDbCommand, string, object> addParameter)
        {
            _getConnection = getConnection;
            _getCommand = getCommand;
            _addParameter = addParameter;
        }

        protected Syntax(Func<string, IDbConnection> getConnection, string connectionString, Func<IDbCommand> getCommand, Action<IDbCommand, string, object> addParameter )
            :this(() => getConnection(connectionString), getCommand, addParameter)
        {
        }

        private readonly List<Action<IDbConnection, IDbTransaction>> _actions = new List<Action<IDbConnection, IDbTransaction>>();

        public IExecutableSyntax Scalar<TReturn>(string query, Action<TReturn> action)
        {
            return Scalar(BuildCommand(query), action);
        }

        public IExecutableSyntax Scalar<TReturn>(string query, IDictionary<string, object> values, Action<TReturn> action)
        {
            return Scalar(BuildCommand(query, values), action);
        }

        public IExecutableSyntax Scalar<TReturn>(Func<IDbCommand> buildCommand, Action<TReturn> action)
        {
            _actions.Add(
                (conn, tx) => ExecuteScalar(conn, tx, buildCommand, action));
            return this;
        }

        public IExecutableSyntax Reader(string query, Action<IDataReader> forEachRow)
        {
            return Reader(BuildCommand(query), forEachRow);
        }

        public IExecutableSyntax Reader(string query, IDictionary<string, object> values, Action<IDataReader> forEachRow)
        {
            return Reader(BuildCommand(query, values), forEachRow);
        }

        public IExecutableSyntax Reader(Func<IDbCommand> buildCommand, Action<IDataReader> forEachRow)
        {
            _actions.Add(
                (conn, tx) => ExecuteReader(conn, tx, buildCommand, forEachRow));
            return this;
        }

        public IExecutableSyntax NonQuery(string query, Action<int> rowsAffected)
        {
            return NonQuery(BuildCommand(query), rowsAffected);
        }

        public IExecutableSyntax NonQuery(string query, IDictionary<string, object> values, Action<int> rowsAffected)
        {
            return NonQuery(BuildCommand(query, values), rowsAffected);
        }

        public IExecutableSyntax NonQuery(Func<IDbCommand> buildCommand, Action<int> rowsAffected)
        {
            _actions.Add(
                (conn, tx) => ExecuteNonQuery(conn, tx, buildCommand, rowsAffected));
            return this;
        }

        public IExecutableSyntax NonQuery(string query)
        {
            return NonQuery(query, rowsAffected => { });
        }

        public IExecutableSyntax NonQuery(string query, IDictionary<string, object> values)
        {
            return NonQuery(query, values, rowsAffected => { });
        }

        public IExecutableSyntax NonQuery(Func<IDbCommand> buildCommand)
        {
            return NonQuery(buildCommand, rowsAffected => { });
        }

        public void Execute()
        {
            using (var conn = _getConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    foreach (var action in _actions)
                        action(conn, tx);
                    tx.Commit();
                }
                conn.Close();
            }
        }

        private Func<IDbCommand> BuildCommand(string query)
        {
            return BuildCommand(query, new Dictionary<string, object>());
        }

        private Func<IDbCommand> BuildCommand(string query, IDictionary<string, object> values)
        {
            var cmdParams = values.ToDictionary(i => i.Key, i => i.Value);
            Func<IDbCommand> builder =
                () =>
                    {
                        var cmd = _getCommand();
                        cmd.CommandText = query;
                        foreach (var paramData in cmdParams)
                            _addParameter(cmd, paramData.Key, paramData.Value);
                        return cmd;
                    };
            return builder;
        }

        private void ExecuteScalar<TResult>(
            IDbConnection connection,
            IDbTransaction transaction,
            Func<IDbCommand> getCommand,
            Action<TResult> action)
        {
            Execute(connection, transaction, getCommand,
                cmd =>
                    {
                        var result = (TResult) cmd.ExecuteScalar();
                        action(result);
                    });
        }

        private void ExecuteReader(
            IDbConnection connection,
            IDbTransaction transaction,
            Func<IDbCommand> getCommand,
            Action<IDataReader> forEachRow)
        {
            Execute(connection, transaction, getCommand,
                cmd =>
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                                forEachRow(rdr);
                            rdr.Close();
                        }
                    });
        }

        private void ExecuteNonQuery(
            IDbConnection connection,
            IDbTransaction transaction,
            Func<IDbCommand> getCommand,
            Action<int> rowsAffectedAction
            )
        {
            Execute(connection, transaction, getCommand,
                cmd =>
                    {
                        try
                        {
                            var rowsAffected = cmd.ExecuteNonQuery();
                            rowsAffectedAction(rowsAffected);
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            Console.WriteLine("Error query: {0}", cmd.CommandText);
                            Console.WriteLine("{0} query parameters", cmd.Parameters.Count);
                            foreach (IDataParameter param in cmd.Parameters)
                                Console.WriteLine("{0} ({1}) = {2}", param.ParameterName, param.DbType, param.Value);
                            throw;
                        }
                    });
        }

        private void Execute(
            IDbConnection connection,
            IDbTransaction transaction,
            Func<IDbCommand> getCommand,
            Action<IDbCommand> action)
        {
            using (var cmd = getCommand())
            {
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                action(cmd);
            }
        }

    }
}
