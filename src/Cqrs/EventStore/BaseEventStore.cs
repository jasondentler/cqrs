using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public abstract class BaseEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly ICommandSender _commandSender;

        protected BaseEventStore(IEventPublisher publisher, ICommandSender commandSender)
        {
            _publisher = publisher;
            _commandSender = commandSender;
        }

        public void SaveEventsFromAggregate(Guid aggregateId,
          IEnumerable<Event> events,
          int expectedVersion)
        {
            var evnts = events.ToArray();
            SaveEvents(aggregateId, evnts, expectedVersion);
            PublishEvents(evnts);
        }

        public void SaveEventsFromSaga(Guid sagaId, IEnumerable<Event> events, int expectedVersion, IEnumerable<Command> dispatches)
        {
            SaveEvents(sagaId, events, expectedVersion);
            DispatchCommands(dispatches);
        }

        private void SaveEvents(Guid eventSourceId, IEnumerable<Event> events, int expectedVersion)
        {
            var eventDescriptors = new List<EventDescriptor>();
            var i = expectedVersion;
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;
                eventDescriptors.Add(new EventDescriptor(eventSourceId, @event, i));
            }

            PersistEventDescriptors(eventDescriptors, eventSourceId, expectedVersion);
        }

        private void PublishEvents(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                _publisher.Publish(@event);
            }
        }

        private void DispatchCommands(IEnumerable<Command> dispatches)
        {
            foreach (var cmd in dispatches)
                _commandSender.Send(cmd);
        }

        public List<Event> GetEvents(Guid eventSourceId)
        {
            var eventDescriptors = LoadEventDescriptorsForAggregate(eventSourceId);
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