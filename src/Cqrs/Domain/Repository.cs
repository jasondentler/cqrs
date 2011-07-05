using System;
using Cqrs.EventStore;
using Cqrs.Sagas;

namespace Cqrs.Domain
{
    public class Repository : IRepository
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(IEventSource eventSource)
        {
            var saga = eventSource as ISaga;
            if (saga != null)
            {
                SaveSaga(saga);
            }
            else
            {
                SaveEventSource(eventSource);
            }
        }

        private void SaveSaga(ISaga saga)
        {
            _storage.SaveEventsFromSaga(saga.Id, saga.GetUncommittedChanges(), saga.Version, saga.GetDispatches());
            saga.MarkChangesAsCommitted();
        }

        private void SaveEventSource(IEventSource eventSource)
        {
            _storage.SaveEventsFromAggregate(eventSource.Id, eventSource.GetUncommittedChanges(), eventSource.Version);
            eventSource.MarkChangesAsCommitted();
        }

        public T GetById<T>(Guid id) where T : class, IEventSource
        {
            var obj = Activator.CreateInstance<T>();//lots of ways to do this
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }

}