using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Commanding;
using Cqrs.EventStore;
using Cqrs.Eventing;
using Cqrs.Sagas;
using NUnit.Framework;
using Rhino.Mocks;
using SharpTestsEx;

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

        [Test]
        public void CanGetById()
        {
            var e1 = new E1();
            var e2 = new E2();
            var history = new Event[] {e1, e2};
            var arId = Guid.NewGuid();

            var mocks = new MockRepository();

            var eventStore = mocks.StrictMock<IEventStore>();

            eventStore.Expect(es => es.GetEvents(arId))
                .Return(history.ToList());

            mocks.ReplayAll();

            var repo = GetInstance(eventStore);
            var ar = repo.GetById<AR>(arId);

            mocks.VerifyAll();

            ar.Events.Should().Have.SameSequenceAs(history);

        }

        public class AR : EventSource
        {

            public readonly List<Event> Events = new List<Event>();

            public AR()
            {
            }

            public AR(Guid someParamsToTry)
            {
                throw new Exception("Wrong constructor!");
            }

            private void Apply(E1 e)
            {
                Events.Add(e);
            }

            private void Apply(E2 e)
            {
                Events.Add(e);
            }

        }

        public class E1 : Event
        {
        }

        public class E2 : Event
        {
        }

    }
}
