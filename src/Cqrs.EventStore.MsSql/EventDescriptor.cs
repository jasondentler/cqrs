using System;

namespace Cqrs.EventStore.MsSql
{
    public class EventDescriptor
    {
        public string EventData { get; private set; }
        public string EventType { get; private set; }
        public Guid Id { get; private set; }
        public int Version { get; private set; }

        public EventDescriptor(
            string eventData,
            string eventType,
            Guid id,
            int version)
        {
            EventData = eventData;
            EventType = eventType;
            Id = id;
            Version = version;
        }
    }
}