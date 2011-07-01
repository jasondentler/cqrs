namespace Cqrs.Eventing
{
    public class NullEventPublisher : IEventPublisher
    {
        public void Publish<TEvent>(TEvent @event) where TEvent : Event
        {
        }
    }
}
