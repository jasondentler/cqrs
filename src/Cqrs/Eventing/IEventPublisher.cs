namespace Cqrs.Eventing
{
    public interface IEventPublisher
    {

        void Publish(Event @event);

    }
}
