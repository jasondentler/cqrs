using System;
using Cqrs.Domain;
using Cqrs.EventStore;

namespace Cqrs.Specs
{
    public class TestRepository<TAggregate> :
        IRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {

        private readonly Repository<TAggregate> _repository;

        public TestRepository(IEventStore eventStore)
        {
            _repository = new Repository<TAggregate>(eventStore);
        }

        public void Save(AggregateRoot aggregate, int expectedVersion)
        {
            foreach (var e in aggregate.GetUncommittedChanges())
                WhenHelper.OnEventStored(e);

            _repository.Save(aggregate, expectedVersion);
        }

        public TAggregate GetById(Guid id)
        {
            return _repository.GetById(id);
        }

    }
}
