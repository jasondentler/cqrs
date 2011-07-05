using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.EventStore;
using Cqrs.Eventing;
using Ninject;
using TechTalk.SpecFlow;

namespace Cqrs.Specs
{
    public class GivenHelper
    {
        private const string EventsKey = "GivenHelper.Events";

        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        private static IEventStore GetEventStore()
        {
            var kernel = Context.Get<IKernel>();
            return kernel.Get<IEventStore>();
        }

        public static void Given(Guid eventSourceId, Event @event)
        {
            var givenEvents = GivenEvents(eventSourceId);

            var expectedVersion = givenEvents.Count();
            @event.Version = expectedVersion + 1;

            var store = GetEventStore();
            store.SaveEventsFromAggregate(eventSourceId, new[] {@event}, expectedVersion);
            var events = GetEvents();
            if (!events.ContainsKey(eventSourceId))
                events[eventSourceId] = new List<Event>();
            events[eventSourceId].Add(@event);
        }

        public static IEnumerable<Event> GivenEvents(Guid eventSourceId)
        {
            var events = GetEvents();
            if (!events.ContainsKey(eventSourceId))
                events[eventSourceId] = new List<Event>();
            return GetEvents()[eventSourceId];
        }

        private static IDictionary<Guid, IList<Event>> GetEvents()
        {
            IDictionary<Guid, IList<Event>> events;
            if (!Context.ContainsKey(EventsKey))
            {
                events = new Dictionary<Guid, IList<Event>>();
                Context[EventsKey] = events;
            }
            else
            {
                events = (IDictionary<Guid, IList<Event>>) Context[EventsKey];
            }
            return events;
        }



    }
}
