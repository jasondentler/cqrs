using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public abstract class BaseEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;

        protected BaseEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public void SaveEvents(Guid aggregateId,
          IEnumerable<Event> events,
          int expectedVersion)
        {
            var eventDescriptors = new List<EventDescriptor>();
            var i = expectedVersion;
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));
            }

            PersistEventDescriptors(eventDescriptors, aggregateId, expectedVersion);

            foreach (var @event in events)
            {
                _publisher.Publish(@event);
            }
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            var eventDescriptors = LoadEventDescriptorsForAggregate(aggregateId);
            if (null == eventDescriptors || !eventDescriptors.Any())
            {
                throw new AggregateNotFoundException();
            }
            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        protected abstract IEnumerable<EventDescriptor>
          LoadEventDescriptorsForAggregate(Guid aggregateId);

        protected abstract void PersistEventDescriptors(
          IEnumerable<EventDescriptor> newEventDescriptors,
          Guid aggregateId,
          int expectedVersion);

    }

}