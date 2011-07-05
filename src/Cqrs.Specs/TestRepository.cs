using System;
using Cqrs.Domain;
using Cqrs.EventStore;
using Cqrs.Sagas;

namespace Cqrs.Specs
{
    public class TestRepository : IRepository
    {

        private readonly Repository _repository;

        public TestRepository(IEventStore eventStore)
        {
            _repository = new Repository(eventStore);
        }

        public void Save(IEventSource eventSource)
        {

            if (eventSource as Saga == null)
                foreach (var e in eventSource.GetUncommittedChanges())
                    WhenHelper.OnEventStored(eventSource.Id, e);

            _repository.Save(eventSource);
        }

        public TEventSource GetById<TEventSource>(Guid id) where TEventSource : class, IEventSource
        {
            return _repository.GetById<TEventSource>(id);
        }

    }
}
