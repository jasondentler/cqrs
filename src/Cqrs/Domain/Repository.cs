using System;
using Cqrs.EventStore;
using Cqrs.Sagas;

namespace Cqrs.Domain
{
    public class Repository<T> : IRepository<T> where T : EventSource, new() //shortcut you can do as you see fit with new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(EventSource eventSource)
        {
            var saga = eventSource as Saga;
            if (saga != null)
            {
                SaveSaga(saga);
            }
            else
            {
                SaveEventSource(eventSource);
            }
        }

        private void SaveSaga(Saga saga)
        {
            _storage.SaveEventsFromSaga(saga.Id, saga.GetUncommittedChanges(), saga.Version, saga.GetDispatches());
            saga.MarkChangesAsCommitted();
        }

        private void SaveEventSource(EventSource eventSource)
        {
            _storage.SaveEventsFromAggregate(eventSource.Id, eventSource.GetUncommittedChanges(), eventSource.Version);
            eventSource.MarkChangesAsCommitted();
        }

        public T GetById(Guid id)
        {
            var obj = new T();//lots of ways to do this
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }

}