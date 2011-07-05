namespace Cqrs.EventStore.MsSql.FluentSqlClient
{
    public interface IExecutableSyntax : ITransactionSyntax
    {

        void Execute();

    }
}
