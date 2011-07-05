using System.Data.SqlClient;

namespace Cqrs.EventStore.MsSql.FluentSqlClient.Impl
{
    public class MsSqlSyntax : Syntax
    {

        public MsSqlSyntax(string connectionString)
            : base(
            () => new SqlConnection(connectionString),
            () => new SqlCommand(),
            (cmd, paramName, value) => ((SqlCommand) cmd).Parameters.AddWithValue(paramName, value))
        {
        }

    }
}
