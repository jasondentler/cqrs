using System;
using System.Collections.Generic;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(Guid aggregateId);
    }
}