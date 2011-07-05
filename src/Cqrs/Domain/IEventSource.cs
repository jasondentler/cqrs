using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrs.Eventing;

namespace Cqrs.Domain
{
    public interface IEventSource
    {

        Guid Id { get; }
        int Version { get; }

        IEnumerable<Event> GetUncommittedChanges();
        void MarkChangesAsCommitted();
        void LoadsFromHistory(IEnumerable<Event> history);


    }
}
