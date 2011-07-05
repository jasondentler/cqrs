namespace Cqrs.Commanding
{
    public interface ICommandSender
    {

        void Send(Command command);

    }
}
