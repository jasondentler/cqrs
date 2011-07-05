using System;
using System.Collections.Generic;
using Cqrs.Commanding;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public interface IEventStore
    {
        void SaveEventsFromAggregate(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);

        void SaveEventsFromSaga(Guid sagaId, IEnumerable<Event> events, int expectedVersion,
                                IEnumerable<Command> dispatches);

        List<Event> GetEventsForAggregate(Guid aggregateId);
    }
}