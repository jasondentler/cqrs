using System;
using Cqrs.Domain;
using Cqrs.EventStore;
using Cqrs.Sagas;

namespace Cqrs.Specs
{
    public class TestRepository<TEventSource> :
        IRepository<TEventSource> where TEventSource : EventSource, new()
    {

        private readonly Repository<TEventSource> _repository;

        public TestRepository(IEventStore eventStore)
        {
            _repository = new Repository<TEventSource>(eventStore);
        }

        public void Save(EventSource eventSource)
        {

            if (eventSource as Saga == null)
                foreach (var e in eventSource.GetUncommittedChanges())
                    WhenHelper.OnEventStored(eventSource.Id, e);

            _repository.Save(eventSource);
        }

        public TEventSource GetById(Guid id)
        {
            return _repository.GetById(id);
        }

    }
}
