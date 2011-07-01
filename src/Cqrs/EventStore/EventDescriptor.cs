using System;
using Cqrs.Eventing;

namespace Cqrs.EventStore
{
    public class EventDescriptor
    {

        public Event EventData { get; private set; }
        public Guid Id { get; private set; }
        public int Version { get; private set; }

        public EventDescriptor(Guid id, Event eventData, int version)
        {
            EventData = eventData;
            Version = version;
            Id = id;
        }

        private EventDescriptor()
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EventDescriptor);
        }

        public bool Equals(EventDescriptor other)
        {
            return null == other
                      ? false
                      : other.Id == Id && other.Version == Version;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Version.GetHashCode();
        }

    }
}