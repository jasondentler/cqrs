using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.Eventing;
using Newtonsoft.Json;
using Ninject;
using TechTalk.SpecFlow;

namespace Cqrs.Specs
{
    public class WhenHelper
    {

        private const string EventsKey = "Events";
        private const string CommandsKey = "Commands";
        private const string ExceptionKey = "Exception";

        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        public static void When<TCommand>(TCommand command) where TCommand : Command
        {
            var kernel = Context.Get<IKernel>();
            var commandSender = kernel.Get<ICommandSender>();
            try
            {
                commandSender.Send(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Context[ExceptionKey] = ex;
            }
        }

        public static Exception Exception { get { return (Exception) Context[ExceptionKey]; } }

        public static bool HasException { get { return Context.ContainsKey(ExceptionKey); } }

        public static Event[] Events
        {
            get { return GetEvents().SelectMany(i => i.Value).ToArray(); }
        }

        public static Event[] GetEvents(Guid eventSourceId)
        {
            return GetEvents()[eventSourceId].ToArray();
        }

        public static Event[] GetEvents<TEventSource>(params string[] naturalId)
        {
            var id = EventSourceHelper.GetId<TEventSource>(naturalId);
            return GetEvents(id);
        }

        internal static void OnEventStored(Guid eventSourceId, Event @event)
        {
            Console.WriteLine("\t\tEvent {0} {1} to {2}", @event.GetType(), JsonConvert.SerializeObject(@event),
                              eventSourceId);

            var events = GetEvents();
            if (!events.ContainsKey(eventSourceId))
            {
                events[eventSourceId] = new List<Event>();
            }
            events[eventSourceId].Add(@event);
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
                events = (IDictionary<Guid, IList<Event>>)Context[EventsKey];
            }
            return events;
        }

        public static Command[] Commands { get { return GetCommandList().ToArray(); } }

        internal static void OnCommandSentk(Command command)
        {

            Console.WriteLine("\tCommand {0} {1}", command.GetType(), JsonConvert.SerializeObject(command));
            GetCommandList().Add(command);
        }

        private static IList<Command> GetCommandList()
        {
            IList<Command> commands;
            if (!Context.ContainsKey(CommandsKey))
            {
                commands = new List<Command>();
                Context[CommandsKey] = commands;
            }
            else
            {
                commands = (IList<Command>)Context[CommandsKey];
            }
            return commands;
        }

    }
}
