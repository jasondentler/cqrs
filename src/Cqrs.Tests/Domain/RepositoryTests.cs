using System;
using Cqrs.Commanding;
using Cqrs.EventStore;
using Cqrs.Eventing;
using Cqrs.Sagas;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cqrs.Domain
{

    public abstract class RepositoryTests<TRepository> 
        where TRepository : IRepository
    {

        protected abstract IRepository GetInstance(IEventStore eventStore);

        [Test]
        public void WhenSavingASaga()
        {
            var mocks = new MockRepository();

            var e1 = mocks.Stub<Event>();
            var e2 = mocks.Stub<Event>();
            var cmd = mocks.Stub<Command>();
            var uncommittedEvents = new[] {e1, e2};
            var dispatches = new[] {cmd};

            var saga = mocks.StrictMock<ISaga>();
            var sagaId = Guid.NewGuid();

            saga.Expect(s => saga.Id)
                .Return(sagaId);

            saga.Expect(s => s.Version)
                .Return(42);

            saga.Expect(s => s.GetUncommittedChanges())
                .Return(uncommittedEvents);
            saga.Expect(s => s.GetDispatches())
                .Return(dispatches);

            saga.Expect(s => s.MarkChangesAsCommitted());

            var eventStore = mocks.StrictMock<IEventStore>();
            eventStore
                .Expect(es => es.SaveEventsFromSaga(sagaId, uncommittedEvents, 42, dispatches));

            mocks.ReplayAll();

            var repo = GetInstance(eventStore);
            repo.Save(saga);

            mocks.VerifyAll();

        }

        [Test]
        public void WhenSavingAnEventSource()
        {
            var mocks = new MockRepository();

            var e1 = mocks.Stub<Event>();
            var e2 = mocks.Stub<Event>();
            var uncommittedEvents = new[] { e1, e2 };

            var eventSource = mocks.StrictMock<IEventSource>();
            var eventSourceId = Guid.NewGuid();

            eventSource.Expect(s => eventSource.Id)
                .Return(eventSourceId);

            eventSource.Expect(s => s.Version)
                .Return(42);

            eventSource.Expect(s => s.GetUncommittedChanges())
                .Return(uncommittedEvents);

            eventSource.Expect(s => s.MarkChangesAsCommitted());

            var eventStore = mocks.StrictMock<IEventStore>();
            eventStore
                .Expect(es => es.SaveEventsFromAggregate(eventSourceId, uncommittedEvents, 42));

            mocks.ReplayAll();

            var repo = GetInstance(eventStore);
            repo.Save(eventSource);

            mocks.VerifyAll();

        }

    }
}
