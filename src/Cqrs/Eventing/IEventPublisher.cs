namespace Cqrs.Eventing
{
    public interface IEventPublisher
    {

        void Publish<TEvent>(TEvent @event) where TEvent : Event;

    }
}
