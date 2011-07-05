using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Eventing;
using DynUnit;

namespace Cqrs.Domain
{
    public abstract class EventSource : IEventSource
    {
        private readonly List<Event> _changes = new List<Event>();

        public virtual Guid Id { get; protected set; }
        public virtual int Version { get; internal set; }

        public virtual IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public virtual void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public virtual void LoadsFromHistory(IEnumerable<Event> history)
        {
            var eventHistory = history.Where(e => e != null).ToArray();
            if (!eventHistory.Any())
                return;
            foreach (var e in eventHistory)
                ApplyChange(e, false);
            Version = eventHistory.Max(e => e.Version);
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}