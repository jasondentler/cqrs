using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public abstract class SerializedEventStore<TSerialized> : BaseEventStore
    {

        private readonly ISerializer<TSerialized> _serializer;

        protected SerializedEventStore(
            IEventPublisher publisher,
            ICommandSender commandSender,
            ISerializer<TSerialized> serializer)
            : base(publisher, commandSender)
        {
            _serializer = serializer;
        }

        protected override IEnumerable<EventDescriptor> LoadEventDescriptorsForAggregate(Guid aggregateId)
        {
            var serializedEvents = LoadSerializedEventsForAggregate(aggregateId);
            return serializedEvents
                .Select(e => _serializer.Deserialize(e))
                .ToArray();
        }

        protected abstract IEnumerable<TSerialized> LoadSerializedEventsForAggregate(Guid aggregateId);

        protected override void PersistEventDescriptors(IEnumerable<EventDescriptor> newEventDescriptors, Guid aggregateId, int expectedVersion)
        {
            var serializedEvents = newEventDescriptors
                .Select(ed => _serializer.Serialize(ed))
                .ToArray();
            PersistSerializedEvents(serializedEvents, aggregateId, expectedVersion);
        }

        protected abstract void PersistSerializedEvents(IEnumerable<TSerialized> serializedEvents, Guid aggregateid,
                                                        int expectedVersion);

    }
}
