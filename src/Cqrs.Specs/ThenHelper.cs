using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Commanding;
using Cqrs.Eventing;
using TechTalk.SpecFlow;

namespace Cqrs.Specs
{
    public class ThenHelper
    {

        private const string CheckedEvents = "CheckedEvents";

        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        private static ISet<Event> GetCheckedEventSet()
        {
            ISet<Event> events;
            if (!Context.ContainsKey(CheckedEvents))
            {
                events = new HashSet<Event>();
                Context[CheckedEvents] = events;
            }
            else
            {
                events = (ISet<Event>)Context[CheckedEvents];
            }
            return events;
        }

        public static TEvent Event<TEvent>() where TEvent : Event
        {
            var e = WhenHelper.Events.OfType<TEvent>().Single();
            GetCheckedEventSet().Add(e);
            return e;
        }

        public static TCommand Command<TCommand>() where TCommand : Command
        {
            return WhenHelper.Commands.OfType<TCommand>().Single();
        }

        public static IEnumerable<Event> UncheckedEvents()
        {
            return WhenHelper.Events.Except(GetCheckedEventSet());
        }

        public static TException Exception<TException>() where TException : Exception
        {
            return (TException)WhenHelper.Exception;
        }

        public static bool HasException()
        {
            return WhenHelper.HasException;
        }

    }
}
