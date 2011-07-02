using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.EventStore;
using Cqrs.Eventing;
using Ninject;
using TechTalk.SpecFlow;

namespace Cqrs.Specs
{
    public class GivenHelper
    {
        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        private static IEventStore GetEventStore()
        {
            var kernel = Context.Get<IKernel>();
            return kernel.Get<IEventStore>();
        }

        public static void Given(Guid eventSourceId, Event @event)
        {
            var store = GetEventStore();
            store.SaveEvents(eventSourceId, new[] {@event}, -1);
        }


    }
}
