namespace Cqrs.Commanding
{
    public interface ICommandSender
    {

        void Send<TCommand>(TCommand command) where TCommand : Command;

    }
}
