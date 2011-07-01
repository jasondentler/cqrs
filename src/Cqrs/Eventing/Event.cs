namespace Cqrs.Eventing
{
    public abstract class Event : IMessage
    {

        public int Version;

    }
}
