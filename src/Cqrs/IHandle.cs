namespace Cqrs
{
    public interface IHandle<TMessage>
        where TMessage : IMessage
    {

        void Handle(TMessage message);

    }
}
