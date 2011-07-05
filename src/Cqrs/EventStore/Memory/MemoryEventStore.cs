using System;
using System.Collections.Generic;
using Cqrs.Commanding;
using Cqrs.Eventing;

namespace Cqrs.EventStore.Memory
{
    public class MemoryEventStore : BaseEventStore
    {

        public MemoryEventStore(IEventPublisher publisher, ICommandSender commandSender)
            : base(publisher, commandSender)
        {
        }

        private readonly Dictionary<Guid, List<EventDescriptor>> _current =
          new Dictionary<Guid, List<EventDescriptor>>();

        protected override void PersistEventDescriptors(
          IEnumerable<EventDescriptor> newEventDescriptors,
          Guid aggregateId, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;
            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }
            eventDescriptors.AddRange(newEventDescriptors);
        }

        protected override IEnumerable<EventDescriptor> LoadEventDescriptorsForAggregate(Guid aggregateId)
        {
            if (!_current.ContainsKey(aggregateId))
                return new EventDescriptor[] { };
            return _current[aggregateId];
        }

    }
}